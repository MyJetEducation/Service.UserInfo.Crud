using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UpdateUserTokenResponse
	{
		[DataMember(Order = 1)]
		public bool IsSuccess { get; set; }
	}
}