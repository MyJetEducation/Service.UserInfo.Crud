using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetUserInfoByNameAsync(string userName);

		ValueTask<UserInfoEntity> GetUserInfoByTokenAsync(string refreshToken);

		ValueTask<bool> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request);

		ValueTask<string> CreateUserInfoAsync(string userName, string password);

		ValueTask<bool> ConfirmUserInfoAsync(string hash);
	}
}