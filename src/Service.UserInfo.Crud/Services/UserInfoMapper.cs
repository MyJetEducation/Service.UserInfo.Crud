using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public static class UserInfoMapper
	{
		public static UserAuthInfoResponse ToGrpcModel(this UserInfoEntity userInfo) =>
			new UserAuthInfoResponse
			{
				UserAuthInfo = userInfo != null
					? new UserAuthInfoGrpcModel
					{
						UserId = userInfo.Id,
						Password = userInfo.Password,
						Role = userInfo.Role,
						RefreshToken = userInfo.RefreshToken,
						RefreshTokenExpires = userInfo.RefreshTokenExpires
					}
					: null
			};
	}
}