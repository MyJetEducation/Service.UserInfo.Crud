using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.Grpc;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Postgres;
using GrpcClientFactory = ProtoBuf.Grpc.Client.GrpcClientFactory;

namespace TestApp
{
	internal class Program
	{
		private static async Task Main()
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;
			ILogger<Program> logger = LoggerFactory.Create(x => x.AddConsole()).CreateLogger<Program>();

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new UserInfoCrudClientFactory("http://localhost:5001", logger);
			IGrpcServiceProxy<IUserInfoService> client = factory.GetUserInfoService();
			IUserInfoService clientService = client.Service;

			//Creating UserInfo
			string userName = $"user-{DateTime.UtcNow:HHmmss}";
			const string password = "123";

			Console.WriteLine($"{Environment.NewLine}Creating UserInfo {userName}.");
			UserIdResponse createResponse = await clientService.CreateUserInfoAsync(new UserInfoRegisterRequest {UserName = userName, Password = password});
			Guid? userId = createResponse.UserId;
			if (userId == null)
			{
				Console.WriteLine("Error! Unable to execute CreateUserInfoAsync");
				Console.ReadLine();
			}
			else
				Console.WriteLine($"Success created user with id {userId} !");

			//Activate UserInfo
			Console.WriteLine($"{Environment.NewLine}Activate UserInfo for {userName}");

			string key = Environment.GetEnvironmentVariable(Service.UserInfo.Crud.Program.EncodingKeyStr);
			var decoder = new EncoderDecoder(key);

			string hash = GetDbContext()
				.UserInfos
				.Where(entity => entity.UserNameHash == decoder.Hash(userName))
				.Select(entity => entity.ActivationHash)
				.FirstOrDefault();
			Console.WriteLine($"{Environment.NewLine}ActivationHash is {hash}");

			CommonGrpcResponse activateResponse = await clientService.ConfirmUserInfoAsync(new UserInfoConfirmRequest {ActivationHash = hash});
			if (!activateResponse.IsSuccess)
			{
				Console.WriteLine("Error! Unable to execute ConfirmUserInfoAsync");
				Console.ReadLine();
			}
			else
				Console.WriteLine("Activated!");

			//Retrieving UserInfo
			Console.WriteLine($"{Environment.NewLine}Retrieving (1) UserInfo by name for {userName}");
			UserInfoResponse getResponse1 = await clientService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = userName});
			LogData(getResponse1);

			//Updating token
			Console.WriteLine($"{Environment.NewLine}Updating token info for {userName}.");
			var refreshToken = Guid.NewGuid().ToString();
			CommonGrpcResponse updateResponse = await clientService.UpdateUserTokenInfoAsync(new UserNewTokenInfoRequest
			{
				UserId = getResponse1.UserInfo.UserId,
				JwtToken = Guid.NewGuid().ToString(),
				RefreshToken = refreshToken,
				RefreshTokenExpires = DateTime.Now,
				IpAddress = "192.168.1.1"
			});

			if (!updateResponse.IsSuccess)
				Console.WriteLine("Error! Unable to execute UpdateUserTokenInfoAsync");
			else
			{
				Console.WriteLine("Success!");

				//Retrieving UserInfo
				Console.WriteLine($"{Environment.NewLine}Retrieving (2) UserInfo by name and password {userName}");
				UserInfoResponse getResponse2 = await clientService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = userName, Password = password});
				LogData(getResponse2);

				if (getResponse2.UserInfo == null)
					Console.WriteLine("Error! Unable to execute (2) GetUserInfoByLoginAsync");

				//Retrieving UserInfo
				Console.WriteLine($"{Environment.NewLine}Retrieving (3) UserInfo by refreshToken {refreshToken}");
				UserInfoResponse getResponse3 = await clientService.GetUserInfoByTokenAsync(new UserInfoTokenRequest {RefreshToken = refreshToken});
				LogData(getResponse3);

				if (getResponse3.UserInfo == null)
					Console.WriteLine("Error! Unable to execute (3) GetUserInfoByTokenAsync");

				//Change user password
				Console.WriteLine($"{Environment.NewLine}ChangePassword");
				CommonGrpcResponse getResponse4 = await clientService.ChangePasswordAsync(new UserInfoChangePasswordRequest {UserName = userName, Password = "newPassword"});
				LogData(getResponse4);

				if (!getResponse4.IsSuccess)
					Console.WriteLine("Error! Unable to execute (3) ChangePasswordAsync");
			}

			Console.ReadLine();
		}

		private static DatabaseContext GetDbContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
			optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=postgres;Database=education");
			var context = new DatabaseContext(optionsBuilder.Options);
			return context;
		}

		private static void LogData(object data) => Console.WriteLine(JsonSerializer.Serialize(data));
	}
}