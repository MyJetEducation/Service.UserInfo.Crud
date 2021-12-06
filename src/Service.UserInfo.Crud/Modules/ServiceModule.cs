using Autofac;
using Service.UserInfo.Crud.Services;

namespace Service.UserInfo.Crud.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserInfoRepository>().AsImplementedInterfaces().SingleInstance();
		}
	}
}