using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public class UserInfoService : IUserInfoService
	{
		private readonly IUserInfoRepository _userInfoRepository;
		private readonly IEncoderDecoder _encoderDecoder;

		public UserInfoService(IUserInfoRepository userInfoRepository, IEncoderDecoder encoderDecoder)
		{
			_userInfoRepository = userInfoRepository;
			_encoderDecoder = encoderDecoder;
		}

		public async ValueTask<UserInfoResponse> GetUserInfoByLoginAsync(UserInfoAuthRequest request)
		{
			string userNameHash = _encoderDecoder.Hash(request.UserName);
			string passwordHash = _encoderDecoder.Hash(request.Password);

			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByLoginAsync(userNameHash, passwordHash);

			return userInfo.ToGrpcModel(_encoderDecoder);
		}

		public async ValueTask<UserInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByTokenAsync(request.RefreshToken);

			return userInfo.ToGrpcModel(_encoderDecoder);
		}

		public async ValueTask<CommonResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request)
		{
			bool isSuccess = await _userInfoRepository.UpdateUserTokenInfoAsync(request);

			return new CommonResponse {IsSuccess = isSuccess};
		}

		public async ValueTask<CommonResponse> CreateUserInfoAsync(UserInfoRegisterRequest request)
		{
			string userNameEncoded = PrepareUserName(request.UserName);
			string userNameHash = _encoderDecoder.Hash(request.UserName);
			string passwordHash = _encoderDecoder.Hash(request.Password);

			string hash = await _userInfoRepository.CreateUserInfoAsync(userNameEncoded, userNameHash, passwordHash);

			//TODO: Here send message to user with hash

			return new CommonResponse {IsSuccess = hash != null};
		}

		private string PrepareUserName(string userName) => _encoderDecoder.Encode(userName.ToLower());

		public async ValueTask<CommonResponse> ConfirmUserInfoAsync(UserInfoConfirmRequest request)
		{
			bool isSuccess = await _userInfoRepository.ConfirmUserInfoAsync(request.Hash);

			return new CommonResponse {IsSuccess = isSuccess};
		}
	}
}