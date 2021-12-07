using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Contracts;

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
		ValueTask<CommonResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request);

		[OperationContract]
		ValueTask<CommonResponse> CreateUserInfoAsync(UserInfoRegisterRequest request);

		[OperationContract]
		ValueTask<CommonResponse> ConfirmUserInfoAsync(UserInfoConfirmRequest request);
	}
}