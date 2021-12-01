using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;

namespace TestApp
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;

			Console.Write("Press enter to start");
			Console.ReadLine();


			var factory = new UserInfoCrudClientFactory("http://localhost:5001");
			IHelloService client = factory.GetHelloService();

			HelloMessage resp = await client.SayHelloAsync(new HelloRequest {Name = "Alex"});
			Console.WriteLine(resp?.Message);

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}