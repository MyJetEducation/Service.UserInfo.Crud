using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoResponse
	{
		[DataMember(Order = 1)]
		public UserInfoGrpcModel UserInfo { get; set; }
	}
}