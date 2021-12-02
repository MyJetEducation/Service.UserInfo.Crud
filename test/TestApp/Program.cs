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

			UserAuthInfoResponse userInfoResponse1 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = "user"});
			Console.WriteLine(JsonSerializer.Serialize(userInfoResponse1));

			Task updateTask = client.UpdateUserTokenInfoAsync(new UserNewTokenInfoRequest
			{
				UserName = "user", 
				JwtToken = Guid.NewGuid().ToString(), 
				RefreshToken = Guid.NewGuid().ToString(), 
				RefreshTokenExpires = DateTime.Now
			});

			updateTask.Wait();

			UserAuthInfoResponse userInfoResponse2 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = "user"});
			Console.WriteLine(JsonSerializer.Serialize(userInfoResponse2));

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}