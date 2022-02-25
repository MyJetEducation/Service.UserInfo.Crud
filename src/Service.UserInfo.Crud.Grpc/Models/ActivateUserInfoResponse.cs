using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class ActivateUserInfoResponse
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }
	}
}