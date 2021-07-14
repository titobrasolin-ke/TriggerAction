using ServiceStack;
using ServiceStack.Api.OpenApi.Specification;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    [Route("/devices", "GET")]
    public class SearchDevices : IReturn<List<Device>>
    {
    }

    [Route("/devices/{DeviceId}", "GET")]
    [DataContract]
    public class DeviceRequest : IReturn<DeviceResponse>
    {
        [ApiMember(
            Name = "DeviceId", DataType = OpenApiType.Integer, Format = OpenApiTypeFormat.Int,
            ParameterType = "path", IsRequired = true, AllowMultiple = false)]
        [DataMember(Name = "DeviceId")]
        public int DeviceId { get; set; }

        [ApiMember(
            Name = "timestamp_lt", DataType = OpenApiType.String, Format = OpenApiTypeFormat.DateTime,
            ParameterType = "query", Description = "Less Than.")]
        [DataMember(Name = "timestamp_lt")]
        public DateTime? TimestampLessThan { get; set; }
    }

    public class Period
    {
        public DateTimeOffset StartTs { get; set; }
        public DateTimeOffset EndTs { get; set; }
    }

    public class DeviceResponse
    {
        public int Id { get; set; }
        public int? PlantId { get; set; }
        public int? DeviceTypeId { get; set; }
        public string SensorLabel { get; set; }
        public string SensorName { get; set; }
        public string UserLabel { get; set; }
        public LocationInfo Location { get; set; }
        public Period Period { get; set; }
        public List<Reading> Values { get; set; }
    }

    [Route("/plants", "GET")]
    public class PlantsRequest : IReturn<List<Plant>>
    {

    }
}
