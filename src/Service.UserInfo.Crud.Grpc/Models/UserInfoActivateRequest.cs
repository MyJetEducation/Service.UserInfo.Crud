using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoActivateRequest
	{
		[DataMember(Order = 1)]
		public Guid? UserId { get; set; }
	}
}