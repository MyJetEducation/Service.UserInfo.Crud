using System.Threading.Tasks;
using Service.UserInfo.Crud.Domain.Models;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Mappers;

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

			return new UserInfoResponse
			{
				UserInfo = userInfo?.ToGrpcModel(_encoderDecoder)
			};
		}

		public async ValueTask<UserInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoByTokenAsync(request.RefreshToken);

			return new UserInfoResponse
			{
				UserInfo = userInfo?.ToGrpcModel(_encoderDecoder)
			};
		}

		public async ValueTask<CommonGrpcResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request)
		{
			bool updated = await _userInfoRepository.UpdateUserTokenInfoAsync(request.UserId, request.JwtToken, request.RefreshToken, request.RefreshTokenExpires, request.IpAddress);

			return CommonGrpcResponse.Result(updated);
		}

		public async ValueTask<CommonGrpcResponse> CreateUserInfoAsync(UserInfoRegisterRequest request)
		{
			string userNameEncoded = PrepareUserName(request.UserName);
			string userNameHash = _encoderDecoder.Hash(request.UserName);
			string passwordHash = _encoderDecoder.Hash(request.Password);

			string hash = await _userInfoRepository.CreateUserInfoAsync(userNameEncoded, userNameHash, passwordHash);

			//TODO: Here send message to user with hash

			return CommonGrpcResponse.Result(hash != null);
		}

		private string PrepareUserName(string userName) => _encoderDecoder.Encode(userName.ToLower());

		public async ValueTask<CommonGrpcResponse> ConfirmUserInfoAsync(UserInfoConfirmRequest request)
		{
			bool confirmed = await _userInfoRepository.ConfirmUserInfoAsync(request.Hash);

			return CommonGrpcResponse.Result(confirmed);
		}
	}
}