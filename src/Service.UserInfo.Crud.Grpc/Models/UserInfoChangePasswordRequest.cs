using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoChangePasswordRequest
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }

		[DataMember(Order = 2)]
		public string Password { get; set; }
	}
}