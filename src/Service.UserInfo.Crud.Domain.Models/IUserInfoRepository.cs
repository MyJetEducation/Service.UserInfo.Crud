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
		/// <param name="userNameEncoded">email (encoded)</param>
		/// <param name="userNameHash">email (hashed)</param>
		/// <param name="passwordHash">password (hashed)</param>
		/// <param name="activationHash">hash to activate user</param>
		ValueTask<bool> CreateUserInfoAsync(string userNameEncoded, string userNameHash, string passwordHash, string activationHash);

		/// <summary>
		///     Confirm user registration (activate UserInfo entity)
		/// </summary>
		/// <param name="activationHash">hash to activate user</param>
		ValueTask<bool> ConfirmUserInfoAsync(string activationHash);

		ValueTask<bool> ChangeUserInfoPasswordAsync(string userNameHash, string passwordHash);
	}
}