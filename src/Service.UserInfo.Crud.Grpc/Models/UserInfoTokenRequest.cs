using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoTokenRequest
	{
		[DataMember(Order = 1)]
		public string RefreshToken { get; set; }
	}
}