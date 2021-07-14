using ServiceStack;
using ServiceStack.Api.OpenApi.Specification;
using System;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel
{
    [Route("/push", "POST")]
    [DataContract]
    public class DeleteRequest : IReturn<TestResponse>
    {
        [ApiMember(Name = "ResourceId", DataType = OpenApiType.String,
            Description = "Identifica univocamente un UrbanDataset prodotto da una specifica Solution producer (sintassi definita nella specifica SCPS Collaboration 2.0)",
            IsRequired = true, AllowMultiple = false)]
        [DataMember(Name = "resource_id")]
        public string ResourceId { get; set; }

        [ApiMember(Name = "ResourceId", DataType = OpenApiType.String, Format = OpenApiTypeFormat.DateTime,
            Description = "Identifica il timestamp di generazione dell'UrbanDataset, utilizzato dal produttore di quel dato in fase di produzione.",
            IsRequired = true, AllowMultiple = false)]
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
