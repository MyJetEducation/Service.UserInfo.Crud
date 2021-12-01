using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public static class UserInfoMapper
	{
		public static UserInfoResponse ToGrpcModel(this UserInfoEntity userInfo) => userInfo == null
			? null
			: new UserInfoResponse
			{
				UserInfo = new UserInfoGrpcModel
				{
					Id = userInfo.Id,
					Email = userInfo.Email,
					FirstName = userInfo.FirstName,
					LastName = userInfo.LastName,
					Sex = userInfo.Sex,
					UserName = userInfo.UserName,
					Role = userInfo.Role,
					JwtToken = userInfo.JwtToken,
					RefreshToken = userInfo.RefreshToken,
					RefreshTokenExpires = userInfo.RefreshTokenExpires
				}
			};
	}
}