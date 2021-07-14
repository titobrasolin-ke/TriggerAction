using ServiceStack;
using ServiceStack.Api.OpenApi.Specification;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    [Route("/basicRequest", "POST")]
    [DataContract]
    public class BasicRequest : IReturn<BasicResponse>
    {
        [ApiMember(
            Name = "resource_id", DataType = OpenApiType.String, Format = OpenApiTypeFormat.DateTime,
            ParameterType = "form")]
        [DataMember(Name = "resource_id")]
        public string ResourceId { get; set; }

        [ApiMember(
            Name = "timestamp_lt", DataType = OpenApiType.String,
            ParameterType = "query", Description = "Less Than.")]
        [DataMember(Name = "timestamp_lt")]
        public DateTime? TimestampLessThan { get; set; }
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
