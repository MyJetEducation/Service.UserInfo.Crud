using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserInfoLoginRequest
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }

		[DataMember(Order = 2)]
		public string Password { get; set; }
	}
}