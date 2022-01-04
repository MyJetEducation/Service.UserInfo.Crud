namespace Service.UserInfo.Crud.Grpc.Models
{
	public interface IUserNamePasswordRequest
	{
		public string UserName { get; set; }

		public string Password { get; set; }
	}
}