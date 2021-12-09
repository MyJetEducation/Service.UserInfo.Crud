using System;
using System.Threading.Tasks;

namespace Service.UserInfo.Crud.Domain.Models
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetUserInfoByLoginAsync(string userNameHash, string passwordHash = null);

		ValueTask<UserInfoEntity> GetUserInfoByTokenAsync(string refreshToken);

		ValueTask<bool> UpdateUserTokenInfoAsync(Guid? userId, string token, string refreshToken, DateTime? refreshTokenExpires, string ipAddress);

		/// <summary>
		///     User registration begins (create not-active UserInfo entity)
		/// </summary>
		/// <param name="userName">email (encoded)</param>
		/// <param name="userNameHash">email (hashed)</param>
		/// <param name="passwordHash">password (hashed)</param>
		/// <returns>Activation hash</returns>
		ValueTask<string> CreateUserInfoAsync(string userName, string userNameHash, string passwordHash);

		/// <summary>
		///     Confirm user registration (activate UserInfo entity)
		/// </summary>
		/// <param name="hash">Activation hash</param>
		/// <returns></returns>
		ValueTask<bool> ConfirmUserInfoAsync(string hash);
	}
}