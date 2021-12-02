using System;
using System.Text.Json;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Contracts;

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

			var userName = $"user-{DateTime.UtcNow:HHmmss}";
			Console.WriteLine($"{Environment.NewLine}Creating UserInfo {userName}.");
			CommonResponse createResponse = await client.CreateUserInfo(new UserInfoRegisterRequest {UserName = userName, Password = "123"});
			if (!createResponse.IsSuccess)
			{
				Console.WriteLine("Error! Unable to execute CreateUserInfo");
				Console.ReadLine();
			}
			else
				Console.WriteLine("Success.");

			Console.WriteLine($"{Environment.NewLine}Retrieving UserId for {userName}");
			UserIdResponse userIdResponse = await client.GetUserIdAsync(new UserInfoLoginRequest {UserName = userName});
			Console.WriteLine(userIdResponse.UserId);

			Console.WriteLine($"{Environment.NewLine}Retrieving (1) UserInfo for {userName}");
			UserAuthInfoResponse getResponse1 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = userName});
			LogData(getResponse1);

			Console.WriteLine($"{Environment.NewLine}Updating token info for {userName}.");
			var refreshToken = Guid.NewGuid().ToString();
			CommonResponse updateResponse = await client.UpdateUserTokenInfoAsync(new UserNewTokenInfoRequest
			{
				UserName = userName,
				JwtToken = Guid.NewGuid().ToString(),
				RefreshToken = refreshToken,
				RefreshTokenExpires = DateTime.Now
			});

			if (!updateResponse.IsSuccess)
				Console.WriteLine("Error! Unable to execute UpdateUserTokenInfoAsync");
			else
			{
				Console.WriteLine("Success.");

				Console.WriteLine($"{Environment.NewLine}Retrieving (2) UserInfo by name {userName}");
				UserAuthInfoResponse getResponse2 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = userName});
				LogData(getResponse2);

				Console.WriteLine($"{Environment.NewLine}Retrieving (3) UserInfo by refreshToken {refreshToken}");
				UserAuthInfoResponse getResponse3 = await client.GetUserInfoByTokenAsync(new UserInfoTokenRequest {RefreshToken = refreshToken});
				LogData(getResponse3);
			}

			Console.ReadLine();
		}

		private static void LogData(object data) => Console.WriteLine(JsonSerializer.Serialize(data));
	}
}