using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.UserInfo.Crud.Grpc;

namespace Service.UserInfo.Crud.Client
{
    [UsedImplicitly]
    public class UserInfoCrudClientFactory: MyGrpcClientFactory
    {
        public UserInfoCrudClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IUserInfoService GetUserInfoService() => CreateGrpcService<IUserInfoService>();
    }
}
