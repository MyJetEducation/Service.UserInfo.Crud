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
		public string Email { get; set; }

		[DataMember(Order = 3)]
		public string FirstName { get; set; }

		[DataMember(Order = 4)]
		public string LastName { get; set; }

		[DataMember(Order = 5)]
		public bool? Sex { get; set; }

		[DataMember(Order = 6)]
		public string UserName { get; set; }

		[DataMember(Order = 7)]
		public string Role { get; set; }

		[DataMember(Order = 8)]
		public string JwtToken { get; set; }

		[DataMember(Order = 9)]
		public string RefreshToken { get; set; }

		[DataMember(Order = 10)]
		public DateTime? RefreshTokenExpires { get; set; }
	}
}