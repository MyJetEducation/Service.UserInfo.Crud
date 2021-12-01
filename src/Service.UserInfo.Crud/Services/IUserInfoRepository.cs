using System.Threading.Tasks;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		Task<UserInfoEntity> GetUserInfoAsync(string userName, string password);

		Task<UserInfoEntity> GetUserInfoAsync(string refreshToken);
	}
}