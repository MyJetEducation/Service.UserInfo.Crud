using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Service.Grpc;
using Service.UserInfo.Crud.Grpc;

namespace Service.UserInfo.Crud.Client
{
	[UsedImplicitly]
	public class UserInfoCrudClientFactory : GrpcClientFactory
	{
		public UserInfoCrudClientFactory(string grpcServiceUrl, ILogger logger) : base(grpcServiceUrl, logger)
		{
		}

		public IGrpcServiceProxy<IUserInfoService> GetUserInfoService() => CreateGrpcService<IUserInfoService>();
	}
}