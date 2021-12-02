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

			UserAuthInfoResponse getResponse1 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = "user"});
			Console.WriteLine(JsonSerializer.Serialize(getResponse1));

			UpdateUserTokenResponse updateResponse = await client.UpdateUserTokenInfoAsync(new UserNewTokenInfoRequest
			{
				UserName = "user",
				JwtToken = Guid.NewGuid().ToString(),
				RefreshToken = Guid.NewGuid().ToString(),
				RefreshTokenExpires = DateTime.Now
			});

			if (!updateResponse.IsSuccess)
				Console.WriteLine("Unable to execute UpdateUserTokenInfoAsync");
			else
			{
				UserAuthInfoResponse getResponse2 = await client.GetUserInfoByLoginAsync(new UserInfoLoginRequest {UserName = "user"});
				Console.WriteLine(JsonSerializer.Serialize(getResponse2));
			}
			
			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}