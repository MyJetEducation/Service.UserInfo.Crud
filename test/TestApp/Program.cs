using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Client;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Postgres;

namespace TestApp
{
	internal class Program
	{
		private static async Task Main()
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new UserInfoCrudClientFactory("http://localhost:5001");
			IUserInfoService client = factory.GetUserInfoService();

			//Creating UserInfo
			var userName = $"user-{DateTime.UtcNow:HHmmss}";
			Console.WriteLine($"{Environment.NewLine}Creating UserInfo {userName}.");
			CommonResponse createResponse = await client.CreateUserInfoAsync(new UserInfoRegisterRequest {UserName = userName, Password = "123"});
			if (!createResponse.IsSuccess)
			{
				Console.WriteLine("Error! Unable to execute CreateUserInfoAsync");
				Console.ReadLine();
			}
			else
				Console.WriteLine("Success.");

			//Activate UserInfo
			Console.WriteLine($"{Environment.NewLine}Activate UserInfo for {userName}");

			string hash = GetDbContext()
				.UserInfos
				.Where(entity => entity.UserName == userName)
				.Select(entity => entity.ActivationHash)
				.FirstOrDefault();
			Console.WriteLine($"{Environment.NewLine}Hash is {hash}");

			CommonResponse activateResponse = await client.ConfirmUserInfoAsync(new UserInfoConfirmRequest {Hash = hash});
			if (!activateResponse.IsSuccess)
			{
				Console.WriteLine("Error! Unable to execute UpdateUserTokenInfoAsync");
				Console.ReadLine();
			}
			else
				Console.WriteLine("Activated!");

			//Retrieving UserId
			Console.WriteLine($"{Environment.NewLine}Retrieving UserId for {userName}");
			UserIdResponse userIdResponse = await client.GetUserIdAsync(new UserInfoLoginRequest {UserName = userName});
			Guid? userId = userIdResponse.UserId;
			Console.WriteLine(userId);

			//Retrieving UserInfo
			Console.WriteLine($"{Environment.NewLine}Retrieving (1) UserInfo for {userName}");
			UserAuthInfoResponse getResponse1 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = userName});
			LogData(getResponse1);

			//Updating token
			Console.WriteLine($"{Environment.NewLine}Updating token info for {userName}.");
			var refreshToken = Guid.NewGuid().ToString();
			CommonResponse updateResponse = await client.UpdateUserTokenInfoAsync(new UserNewTokenInfoRequest
			{
				UserId = userId,
				JwtToken = Guid.NewGuid().ToString(),
				RefreshToken = refreshToken,
				RefreshTokenExpires = DateTime.Now,
				IpAddress = "192.168.1.1"
			});

			if (!updateResponse.IsSuccess)
				Console.WriteLine("Error! Unable to execute UpdateUserTokenInfoAsync");
			else
			{
				Console.WriteLine("Success.");

				//Retrieving UserInfo
				Console.WriteLine($"{Environment.NewLine}Retrieving (2) UserInfo by name {userName}");
				UserAuthInfoResponse getResponse2 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = userName});
				LogData(getResponse2);

				//Retrieving UserInfo
				Console.WriteLine($"{Environment.NewLine}Retrieving (3) UserInfo by refreshToken {refreshToken}");
				UserAuthInfoResponse getResponse3 = await client.GetUserInfoByTokenAsync(new UserInfoTokenRequest {RefreshToken = refreshToken});
				LogData(getResponse3);
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