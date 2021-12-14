using Autofac;
using Service.Core.Domain;
using Service.Core.Domain.Models;
using Service.UserInfo.Crud.Domain;

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