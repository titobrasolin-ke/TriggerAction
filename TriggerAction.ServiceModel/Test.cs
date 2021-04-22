using ServiceStack;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel
{
    [Route("/test", "GET,POST")]
    public class TestRequest : IReturn<TestResponse>
    {
    }

    [DataContract]
    public class TestResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
