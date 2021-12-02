using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserIdResponse
	{
		[DataMember(Order = 1)]
		public Guid? UserId { get; set; }
	}
}