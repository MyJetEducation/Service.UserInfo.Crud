using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Contracts;

namespace Service.UserInfo.Crud.Grpc
{
	[ServiceContract]
	public interface IUserInfoService
	{
		[OperationContract]
		ValueTask<UserAuthInfoResponse> GetUserInfoByLoginAsync(UserInfoLoginRequest request);

		[OperationContract]
		ValueTask<UserIdResponse> GetUserIdAsync(UserInfoLoginRequest request);

		[OperationContract]
		ValueTask<CommonResponse> UpdateUserTokenInfoAsync(UserNewTokenInfoRequest request);

		[OperationContract]
		ValueTask<CommonResponse> CreateUserInfo(UserInfoRegisterRequest request);
	}
}