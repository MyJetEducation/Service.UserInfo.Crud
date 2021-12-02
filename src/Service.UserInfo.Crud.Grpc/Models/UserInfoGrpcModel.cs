using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public Guid? Id { get; set; }

		[DataMember(Order = 2)]
		public string FirstName { get; set; }

		[DataMember(Order = 3)]
		public string LastName { get; set; }

		[DataMember(Order = 4)]
		public bool? Sex { get; set; }

		[DataMember(Order = 5)]
		public string UserName { get; set; }

		[DataMember(Order = 6)]
		public string Role { get; set; }
	}
}