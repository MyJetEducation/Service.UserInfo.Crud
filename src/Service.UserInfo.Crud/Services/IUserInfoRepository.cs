using System;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetUserInfoAsync(string userName);

		ValueTask<bool> UpdateUserTokenInfoAsync(string userName, string jwtToken, string refreshToken, DateTime? refreshTokenExpires);
	}
}