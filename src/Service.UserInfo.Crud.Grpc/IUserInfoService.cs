using System.ServiceModel;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserInfo.Crud.Grpc
{
	[ServiceContract]
	public interface IUserInfoService
	{
		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByLoginAsync(UserInfoAuthRequest request);

		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByIdAsync(UserInfoRequest request);

		[OperationContract]
		ValueTask<UserIdResponse> CreateUserInfoAsync(UserInfoRegisterRequest request);

		[OperationContract]
		ValueTask<ActivateUserInfoResponse> ActivateUserInfoAsync(UserInfoActivateRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> ChangePasswordAsync(UserInfoChangePasswordRequest request);

		[OperationContract]
		ValueTask<CommonGrpcResponse> ChangeUserNameAsync(ChangeUserNameRequest request);
	}
}