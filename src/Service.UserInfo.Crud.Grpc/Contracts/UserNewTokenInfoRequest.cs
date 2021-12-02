using System;
using System.Runtime.Serialization;

namespace Service.UserInfo.Crud.Grpc.Contracts
{
	[DataContract]
	public class UserNewTokenInfoRequest
	{
		[DataMember(Order = 1)]
		public string UserName { get; set; }

		[DataMember(Order = 2)]
		public string JwtToken { get; set; }

		[DataMember(Order = 3)]
		public string RefreshToken { get; set; }

		[DataMember(Order = 4)]
		public DateTime? RefreshTokenExpires { get; set; }
	}
}