using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoConfirmRequest
	{
		[DataMember(Order = 1)]
		public string ActivationHash { get; set; }
	}
}