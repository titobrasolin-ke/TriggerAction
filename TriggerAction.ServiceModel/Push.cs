using ServiceStack;
using System.Runtime.Serialization;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    [Route("/push", "POST")]
    [DataContract]
    public class PushRequest : IReturn<PushResponse>
    {
        [DataMember(Name ="resource_id")]
        public string ResourceId { get; set; }
        [DataMember(Name = "dataset")]
        public Template Dataset { get; set; }
    }

    [DataContract]
    public class PushResponse : TestResponse
    {
        [DataMember(Name = "detail")]
        public string Detail { get; set; }
    }
}
