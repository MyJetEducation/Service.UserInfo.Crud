using Autofac;
using Service.UserInfo.Crud.Domain;
using Service.UserInfo.Crud.Domain.Models;

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