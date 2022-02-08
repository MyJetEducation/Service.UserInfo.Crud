using Service.Core.Client.Services;
using Service.UserInfo.Crud.Grpc.Models;
using Service.UserInfo.Crud.Postgres.Models;

namespace Service.UserInfo.Crud.Mappers
{
	public static class UserInfoMapper
	{
		public static UserInfoGrpcModel ToGrpcModel(this UserInfoEntity userInfo, IEncoderDecoder encoderDecoder) => new UserInfoGrpcModel
		{
			UserId = userInfo.Id,
			UserName = encoderDecoder.Decode(userInfo.UserName),
			Role = userInfo.Role,
			RefreshToken = userInfo.RefreshToken,
			RefreshTokenExpires = userInfo.RefreshTokenExpires,
			IpAddress = userInfo.IpAddress
		};
	}
}