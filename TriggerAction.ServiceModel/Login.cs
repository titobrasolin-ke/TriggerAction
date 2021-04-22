using ServiceStack;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel
{
    [Route("/login", "POST")]
    [DataContract]
    public class LoginRequest : IReturn<LoginResponse>
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }

    [DataContract]
    public class LoginResponse : TestResponse
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
