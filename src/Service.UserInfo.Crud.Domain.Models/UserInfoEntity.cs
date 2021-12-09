using System;

namespace Service.UserInfo.Crud.Domain.Models
{
	public class UserInfoEntity
	{
		public Guid? Id { get; set; }

		public string UserName { get; set; }
		
		public string UserNameHash { get; set; }

		public string PasswordHash { get; set; }

		public string Role { get; set; }

		public string JwtToken { get; set; }

		public string RefreshToken { get; set; }

		public DateTime? RefreshTokenExpires { get; set; }

		public string IpAddress { get; set; }

		public string ActivationHash { get; set; }
	}
}