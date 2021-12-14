using System.ServiceModel;
using System.Threading.Tasks;
using Service.Core.Grpc.Models;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserInfo.Crud.Grpc
{
	[ServiceContract]
	public interface IUserInfoService
	{
		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByLoginAsync(UserInfoAuthRequest request);

		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> CreateUserInfoAsync(UserInfoRegisterRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> ConfirmUserInfoAsync(UserInfoConfirmRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> ChangePasswordAsync(UserInfoChangePasswordRequest request);
	}
}