using System;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetByLoginAsync(string userNameHash, string passwordHash = null);
		ValueTask<UserInfoEntity> GetByIdAsync(Guid? userId);

		/// <summary>
		///     User registration begins (create not-active UserInfo entity)
		/// </summary>
		/// <param name="userNameEncoded">email (encoded)</param>
		/// <param name="userNameHash">email (hashed)</param>
		/// <param name="passwordHash">password (hashed)</param>
		ValueTask<Guid?> CreateAsync(string userNameEncoded, string userNameHash, string passwordHash);

		/// <summary>
		///     Confirm user registration (activate UserInfo entity)
		/// </summary>
		/// <param name="userId">userId</param>
		ValueTask<string> ActivateAsync(Guid? userId);

		ValueTask<bool> ChangePasswordAsync(string userNameHash, string passwordHash);

		ValueTask<bool> ChangeUserNameAsync(Guid? userId, string userNameEncoded, string userNameHash);
	}
}