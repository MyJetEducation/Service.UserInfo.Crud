using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public interface IUserInfoRepository
	{
		ValueTask<UserInfoEntity> GetUserInfoByLoginAsync(string userNameHash, string passwordHash = null);

		ValueTask<UserInfoEntity> GetUserInfoByTokenAsync(string refreshToken);

		ValueTask<bool> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request);

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