using System;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		Task<UserInfoEntity> GetUserInfoAsync(string userName);

		Task UpdateUserTokenInfoAsync(string userName, string jwtToken, string refreshToken, DateTime? refreshTokenExpires);
	}
}