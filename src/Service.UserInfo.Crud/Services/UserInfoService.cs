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

		public async ValueTask<UserInfoResponse> GetUserInfoByLoginAsync(UserInfoLoginRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoAsync(request.UserName, request.Password);

			return userInfo.ToGrpcModel();
		}

		public async ValueTask<UserInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoAsync(request.RefreshToken);

			return userInfo.ToGrpcModel();
		}
	}
}