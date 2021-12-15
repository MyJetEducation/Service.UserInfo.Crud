using System.Threading.Tasks;
using Service.Core.Domain.Models;
using Service.Core.Grpc.Models;
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
			string userNameHash = GetHash(request.UserName);
			string passwordHash = GetHash(request.Password);

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
			string userNameHash = GetHash(request.UserName);
			string passwordHash = GetHash(request.Password);
			string userNameEncoded = _encoderDecoder.Encode(request.UserName.ToLower());

			bool created = await _userInfoRepository.CreateUserInfoAsync(userNameEncoded, userNameHash, passwordHash, request.ActivationHash);

			return CommonGrpcResponse.Result(created);
		}

		public async ValueTask<CommonGrpcResponse> ChangePasswordAsync(UserInfoChangePasswordRequest request)
		{
			string userNameHash = GetHash(request.UserName);
			string passwordHash = GetHash(request.Password);

			bool changed = await _userInfoRepository.ChangeUserInfoPasswordAsync(userNameHash, passwordHash);

			return CommonGrpcResponse.Result(changed);
		}

		private string GetHash(string value) => _encoderDecoder.Hash(value);

		public async ValueTask<CommonGrpcResponse> ConfirmUserInfoAsync(UserInfoConfirmRequest request)
		{
			bool confirmed = await _userInfoRepository.ConfirmUserInfoAsync(request.ActivationHash);

			return CommonGrpcResponse.Result(confirmed);
		}
	}
}