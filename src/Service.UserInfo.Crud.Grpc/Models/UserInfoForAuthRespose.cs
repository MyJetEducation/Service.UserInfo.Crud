using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoForAuthRespose
	{
		[DataMember(Order = 1)]
		public UserInfoGrpcModel UserInfo { get; set; }

		[DataMember(Order = 2)]
		public bool UserNotFound { get; set; }

		[DataMember(Order = 3)]
		public bool InvalidPassword { get; set; }

		public bool IsValid() => !UserNotFound && !InvalidPassword;
	}
}