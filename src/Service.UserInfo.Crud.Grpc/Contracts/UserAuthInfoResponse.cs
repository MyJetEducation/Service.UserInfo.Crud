using System.Runtime.Serialization;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserAuthInfoResponse
	{
		[DataMember(Order = 1)]
		public UserAuthInfoGrpcModel UserAuthInfo { get; set; }
	}
}