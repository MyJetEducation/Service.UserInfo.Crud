namespace Service.UserInfo.Crud.Postgres.Models
{
	public class UserInfoEntity
	{
		public Guid? Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool? Sex { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }

		public string JwtToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime? RefreshTokenExpires { get; set; }
		public string IpAddress { get; set; }

		public string ActivationHash { get; set; }
	}
}