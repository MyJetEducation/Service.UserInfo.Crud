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
		private static async Task Main(string[] args)
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new UserInfoCrudClientFactory("http://localhost:80");
			IUserInfoService client = factory.GetUserInfoService();

			UserInfoResponse userInfoResponse1 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = "user", Password = "123"});
			Console.WriteLine(JsonSerializer.Serialize(userInfoResponse1));

			UserInfoResponse userInfoResponse2 = await client.GetUserInfoByTokenAsync(new UserInfoTokenRequest {RefreshToken = "token"});
			Console.WriteLine(JsonSerializer.Serialize(userInfoResponse2));

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}