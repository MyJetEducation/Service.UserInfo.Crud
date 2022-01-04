using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoRegisterRequest : IUserNamePasswordRequest
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }
		
		[DataMember(Order = 2)]
		public string Password { get; set; }

		[DataMember(Order = 3)]
		public string ActivationHash { get; set; }
	}
}