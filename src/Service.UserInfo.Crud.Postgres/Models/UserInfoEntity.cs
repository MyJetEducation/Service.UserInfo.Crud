namespace Service.UserInfo.Crud.Postgres.Models
{
	public class UserInfoEntity
	{
		public Guid? Id { get; set; }

		public string UserName { get; set; }
		
		public string UserNameHash { get; set; }

		public string PasswordHash { get; set; }

		public string Role { get; set; }

		public bool? Active { get; set; }
	}
}