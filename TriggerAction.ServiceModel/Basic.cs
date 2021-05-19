using ServiceStack;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    [Route("/basicRequest", "POST")]
    public class BasicRequest : IReturn<BasicResponse>
    {
        [DataMember(Name = "resource_id")]
        public string ResourceId { get; set; }
    }

    [DataContract]
    public class BasicResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "dataset")]
        public List<Template> Dataset { get; set; }
    }
}
