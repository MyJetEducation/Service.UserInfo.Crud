using Service.UserInfo.Crud.Grpc.Contracts;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Services
{
	public static class UserInfoMapper
	{
		public static UserInfoResponse ToGrpcModel(this UserInfoEntity userInfo, IEncoderDecoder encoderDecoder) => new UserInfoResponse
		{
			UserInfo = userInfo != null
				? new UserInfoGrpcModel
				{
					UserId = userInfo.Id,
					UserName = encoderDecoder.Decode(userInfo.UserName),
					Role = userInfo.Role,
					RefreshToken = userInfo.RefreshToken,
					RefreshTokenExpires = userInfo.RefreshTokenExpires,
					IpAddress = userInfo.IpAddress
				}
				: null
		};
	}
}