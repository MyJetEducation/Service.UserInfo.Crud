using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Contracts;

namespace Service.UserInfo.Crud.Grpc
{
	[ServiceContract]
	public interface IUserInfoService
	{
		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByLoginAsync(UserInfoLoginRequest request);

		[OperationContract]
		ValueTask<UserInfoResponse> GetUserInfoByTokenAsync(UserInfoTokenRequest request);
	}
}