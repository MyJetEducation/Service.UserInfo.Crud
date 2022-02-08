using Autofac;
using Service.Core.Client.Services;
using Service.UserInfo.Crud.Services;

namespace Service.UserInfo.Crud.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserInfoRepository>().AsImplementedInterfaces().SingleInstance();

			builder.Register(context => new EncoderDecoder(Program.EncodingKey))
				.As<IEncoderDecoder>()
				.SingleInstance();
		}
	}
}