using System;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetUserInfoByNameAsync(string userName);

		ValueTask<UserInfoEntity> GetUserInfoByTokenAsync(string refreshToken);

		ValueTask<bool> UpdateUserTokenInfoAsync(string userName, string jwtToken, string refreshToken, DateTime? refreshTokenExpires, string ipAddress);

		ValueTask<bool> CreateUserInfo(string userName, string password);
	}
}