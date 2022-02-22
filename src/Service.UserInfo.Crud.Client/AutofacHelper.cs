using Autofac;
using Microsoft.Extensions.Logging;
using Service.Grpc;
using Service.UserInfo.Crud.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.UserInfo.Crud.Client
{
	public static class AutofacHelper
	{
		public static void RegisterUserInfoCrudClient(this ContainerBuilder builder, string grpcServiceUrl, ILogger logger)
		{
			var factory = new UserInfoCrudClientFactory(grpcServiceUrl, logger);

			builder.RegisterInstance(factory.GetUserInfoService()).As<IGrpcServiceProxy<IUserInfoService>>().SingleInstance();
		}
	}
}