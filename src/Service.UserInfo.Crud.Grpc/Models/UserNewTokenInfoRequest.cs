using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Models
{
	[DataContract]
	public class UserNewTokenInfoRequest
	{
		[DataMember(Order = 1)]
		public Guid? UserId { get; set; }

		[DataMember(Order = 2)]
		public string JwtToken { get; set; }

		[DataMember(Order = 3)]
		public string RefreshToken { get; set; }

		[DataMember(Order = 4)]
		public DateTime? RefreshTokenExpires { get; set; }

		[DataMember(Order = 6)]
		public string IpAddress { get; set; }
	}
}