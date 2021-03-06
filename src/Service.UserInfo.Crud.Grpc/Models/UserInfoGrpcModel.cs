using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public Guid? UserId { get; set; }

		[DataMember(Order = 2)]
		public string UserName { get; set; }

		[DataMember(Order = 3)]
		public string Role { get; set; }
	}
}