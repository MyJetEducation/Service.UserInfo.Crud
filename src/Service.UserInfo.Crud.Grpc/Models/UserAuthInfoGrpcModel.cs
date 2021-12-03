using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserAuthInfoGrpcModel
	{
		[DataMember(Order = 1)]
		public Guid? UserId  { get; set; }

		[DataMember(Order = 2)]
		public string UserName { get; set; }

		[DataMember(Order = 3)]
		public string Password { get; set; }

		[DataMember(Order = 4)]
		public string Role { get; set; }

		[DataMember(Order = 5)]
		public string RefreshToken { get; set; }

		[DataMember(Order = 6)]
		public DateTime? RefreshTokenExpires { get; set; }

		[DataMember(Order = 7)]
		public string IpAddress { get; set; }
	}
}