using System;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.UserInfo.Crud.Grpc;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Mappers;
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
			(string userNameHash, string passwordHash) = GetHashes(request);

			UserInfoEntity userInfo = await _userInfoRepository.GetByLoginAsync(userNameHash, passwordHash);

			return new UserInfoResponse
			{
				UserInfo = userInfo?.ToGrpcModel(_encoderDecoder)
			};
		}

		public async ValueTask<UserInfoResponse> GetUserInfoByIdAsync(UserInfoRequest request)
		{
			UserInfoEntity userInfo = await _userInfoRepository.GetByIdAsync(request.UserId);

			return new UserInfoResponse
			{
				UserInfo = userInfo?.ToGrpcModel(_encoderDecoder)
			};
		}

		public async ValueTask<UserIdResponse> CreateUserInfoAsync(UserInfoRegisterRequest request)
		{
			(string userNameHash, string passwordHash) = GetHashes(request);
			string userNameEncoded = _encoderDecoder.Encode(request.UserName.ToLower());

			Guid? userId = await _userInfoRepository.CreateAsync(userNameEncoded, userNameHash, passwordHash);

			return new UserIdResponse {UserId = userId};
		}

		public async ValueTask<CommonGrpcResponse> ChangePasswordAsync(UserInfoChangePasswordRequest request)
		{
			(string userNameHash, string passwordHash) = GetHashes(request);

			bool changed = await _userInfoRepository.ChangePasswordAsync(userNameHash, passwordHash);

			return CommonGrpcResponse.Result(changed);
		}

		public async ValueTask<ActivateUserInfoResponse> ActivateUserInfoAsync(UserInfoActivateRequest request)
		{
			string userName = await _userInfoRepository.ActivateAsync(request.UserId);

			return new ActivateUserInfoResponse
			{
				UserName = userName
			};
		}

		public async ValueTask<CommonGrpcResponse> ChangeUserNameAsync(ChangeUserNameRequest request)
		{
			string email = request.Email;
			string userNameHash = GetHash(email.ToLower());
			string userNameEncoded = _encoderDecoder.Encode(email.ToLower());

			bool changed = await _userInfoRepository.ChangeUserNameAsync(request.UserId, userNameEncoded, userNameHash);

			return CommonGrpcResponse.Result(changed);
		}

		private (string userNameHash, string passwordHash) GetHashes(IUserNamePasswordRequest request) => (GetHash(request.UserName.ToLower()), GetHash(request.Password));

		private string GetHash(string value) => _encoderDecoder.Hash(value);
	}
}