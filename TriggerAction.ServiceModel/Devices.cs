using ServiceStack;
using System;
using System.Collections.Generic;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    [Route("/devices/{DeviceId}", "GET")]
    public class DeviceRequest : IReturn<DeviceResponse>
    {
        public int DeviceId { get; set; }
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
        public string SensorTypeCode { get; set; }
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
