using ServiceStack;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel
{
    [Route("/isAlive")]
    [DataContract]
    public class IsAliveRequest : IReturn<IsAliveResponse>
    {
    }

    [DataContract]
    public class IsAliveResponse : TestResponse
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }
    }
}
