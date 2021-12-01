using System.Runtime.Serialization;
using Service.UserInfo.Crud.Domain.Models;

namespace Service.UserInfo.Crud.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}