using Autofac;
using Service.UserInfo.Crud.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.UserInfo.Crud.Client
{
    public static class AutofacHelper
    {
        public static void RegisterUserInfoCrudClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new UserInfoCrudClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetUserInfoService()).As<IUserInfoService>().SingleInstance();
        }
    }
}
