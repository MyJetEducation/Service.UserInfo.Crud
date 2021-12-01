using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserInfo.Crud.Grpc.Models;

namespace Service.UserInfo.Crud.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}