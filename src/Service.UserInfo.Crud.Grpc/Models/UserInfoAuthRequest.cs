using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoAuthRequest : IUserNamePasswordRequest
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }

		[DataMember(Order = 2, IsRequired = false)]
		public string Password { get; set; }
	}
}