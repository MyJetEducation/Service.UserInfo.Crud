﻿using System.Threading.Tasks;
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
			UserInfoEntity userInfo = await _userInfoRepository.GetUserInfoAsync(request.UserName);

			return userInfo.ToGrpcModel();
		}

		public async ValueTask<UpdateUserTokenResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request)
		{
			bool isSuccess = await _userInfoRepository.UpdateUserTokenInfoAsync(request.UserName, request.JwtToken, request.RefreshToken, request.RefreshTokenExpires);

			return new UpdateUserTokenResponse {IsSuccess = isSuccess};
		}
	}
}