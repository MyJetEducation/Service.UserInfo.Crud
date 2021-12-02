using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserAuthInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public string Password { get; set; }

		[DataMember(Order = 2)]
		public string Role { get; set; }

		[DataMember(Order = 3)]
		public string RefreshToken { get; set; }

		[DataMember(Order = 4)]
		public DateTime? RefreshTokenExpires { get; set; }
	}
}