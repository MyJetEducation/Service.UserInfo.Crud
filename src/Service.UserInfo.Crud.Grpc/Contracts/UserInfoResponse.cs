using System.Runtime.Serialization;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserInfoResponse
	{
		[DataMember(Order = 1)]
		public UserInfoGrpcModel UserInfo { get; set; }
	}
}