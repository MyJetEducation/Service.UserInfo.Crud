using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class CommonResponse
	{
		[DataMember(Order = 1)]
		public bool IsSuccess { get; set; }
	}
}