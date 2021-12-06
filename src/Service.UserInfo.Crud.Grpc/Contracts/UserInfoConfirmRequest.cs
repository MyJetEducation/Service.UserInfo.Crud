using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserInfoConfirmRequest
	{
		[DataMember(Order = 1)]
		public string Hash { get; set; }
	}
}