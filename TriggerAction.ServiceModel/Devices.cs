using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceModel
{
    public enum SensorTypeCodes
    {
        [EnumMember(Value = "Smart Plug")]
        SmartPlug = 1,
        [EnumMember(Value = "Smart Switch")]
        SmartSwitch = 2,
        [EnumMember(Value = "Smart Presence")]
        SmartPresence = 3,
        [EnumMember(Value = "Confort Multisensor")]
        ConfortMultisensor = 4,
        [EnumMember(Value = "Weather Multisensor")]
        WeatherMultisensor = 5,
    }

    [Route("/devices/{DeviceId}", "GET")]
    public class DeviceRequest : IReturn<DeviceResponse>
    {
        public int DeviceId { get; set; }
    }

    // http://opendustmap.github.io/dustduino-server/

    public class Period
    {
        public DateTimeOffset StartTs { get; set; }
        public DateTimeOffset EndTs { get; set; }
    }

    public class DeviceResponse
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int DeviceTypeId { get; set; }
        public string SensorLabel { get; set; }
        public string SensorName { get; set; }
        public string SensorTypeCode { get; set; }
        public string UserLabel { get; set; }
        public string Location { get; set; }
        public Period Period { get; set; }
        public List<Reading> Values { get; set; }
    }

    [Route("/plants", "GET")]
    public class PlantsRequest : IReturn<List<Plant>>
    {

    }
}
