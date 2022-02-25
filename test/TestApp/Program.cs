using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Extensions;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;
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

			string userName = $"user-{DateTime.UtcNow:HHmmss}";
			const string password = "123";

			//Creating UserInfo
			Console.WriteLine($"{Environment.NewLine}Creating UserInfo {userName}.");
			UserIdResponse createResponse = await clientService.CreateUserInfoAsync(new UserInfoRegisterRequest {UserName = userName, Password = password});
			Guid? userId = createResponse.UserId;
			if (userId == null)
				throw new Exception("Error! Unable to execute CreateUserInfoAsync");
			Console.WriteLine($"Success created user with id {userId} !");

			//Activate UserInfo
			Console.WriteLine($"{Environment.NewLine}Activate UserInfo for {userName}");
			ActivateUserInfoResponse activateResponse = await clientService.ActivateUserInfoAsync(new UserInfoActivateRequest {UserId = userId});
			if (!activateResponse.UserName.IsNullOrEmpty())
				throw new Exception("Error! Unable to execute ConfirmUserInfoAsync");
			Console.WriteLine("Activated!");

			//Retrieving UserInfo by name
			Console.WriteLine($"{Environment.NewLine}Retrieving (1) UserInfo by name for {userName}");
			UserInfoResponse getResponse1 = await clientService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = userName});
			if (getResponse1.UserInfo == null)
				throw new Exception("Error! Unable to execute GetUserInfoByLoginAsync");
			LogData(getResponse1);

			//Retrieving UserInfo by id
			Console.WriteLine($"{Environment.NewLine}Retrieving UserInfo by id for {userId}");
			UserInfoResponse getResponse12 = await clientService.GetUserInfoByIdAsync(new UserInfoRequest {UserId = userId});
			if (getResponse12.UserInfo == null)
				throw new Exception("Error! Unable to execute GetUserInfoByIdAsync");
			LogData(getResponse12);

			//Retrieving UserInfo
			Console.WriteLine($"{Environment.NewLine}Retrieving (2) UserInfo by name and password {userName}");
			UserInfoResponse getResponse2 = await clientService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = userName, Password = password});
			if (getResponse2.UserInfo == null)
				throw new Exception("Error! Unable to execute GetUserInfoByIdAsync");
			LogData(getResponse2);

			//Change user password
			Console.WriteLine($"{Environment.NewLine}ChangePassword");
			CommonGrpcResponse getResponse4 = await clientService.ChangePasswordAsync(new UserInfoChangePasswordRequest {UserName = userName, Password = "newPassword"});
			if (!getResponse4.IsSuccess)
				throw new Exception("Error! Unable to execute (3) ChangePasswordAsync");
			LogData(getResponse4);

			//Change user login
			Console.WriteLine($"{Environment.NewLine}ChangeLogin");
			string newUserName = $"newuser-{DateTime.UtcNow:HHmmss}";
			CommonGrpcResponse getResponse5 = await clientService.ChangeUserNameAsync(new ChangeUserNameRequest {UserId = userId, Email = newUserName});
			if (!getResponse5.IsSuccess)
				throw new Exception("Error! Unable to execute ChangeLoginAsync");
			LogData(getResponse5);

			//Retrieving UserInfo
			Console.WriteLine($"{Environment.NewLine}Retrieving (4) UserInfo by new userName {newUserName}");
			UserInfoResponse getResponse6 = await clientService.GetUserInfoByLoginAsync(new UserInfoAuthRequest {UserName = newUserName});
			if (getResponse6.UserInfo?.UserName != newUserName)
				throw new Exception("Can't change user login!");
			LogData(getResponse6);

			Console.ReadLine();
		}

		private static void LogData(object data) => Console.WriteLine(JsonSerializer.Serialize(data));
	}
}