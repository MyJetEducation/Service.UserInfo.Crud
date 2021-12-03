using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public class UserInfoService : IUserInfoService
	{
		private readonly IUserInfoRepository _userInfoRepository;

		public UserInfoService(IUserInfoRepository userInfoRepository) => _userInfoRepository = userInfoRepository;

		public async ValueTask<UserAuthInfoResponse> GetUserInfoByLoginAsync(UserInfoLoginRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByNameAsync(request.UserName);

			return userInfo.ToGrpcModel();
		}

		public async ValueTask<UserAuthInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByTokenAsync(request.RefreshToken);

			return userInfo.ToGrpcModel();
		}

		public async ValueTask<UserIdResponse> GetUserIdAsync(UserInfoLoginRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByNameAsync(request.UserName);

			return new UserIdResponse {UserId = userInfo?.Id};
		}

		public async ValueTask<CommonResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request)
		{
			bool isSuccess = await _userInfoRepository.UpdateUserTokenInfoAsync(request.UserName, request.JwtToken, request.RefreshToken, request.RefreshTokenExpires, request.IpAddress);

			return new CommonResponse {IsSuccess = isSuccess};
		}

		public async ValueTask<CommonResponse> CreateUserInfo(UserInfoRegisterRequest request)
		{
			bool isSuccess = await _userInfoRepository.CreateUserInfo(request.UserName, request.Password);

			return new CommonResponse {IsSuccess = isSuccess};
		}
	}
}