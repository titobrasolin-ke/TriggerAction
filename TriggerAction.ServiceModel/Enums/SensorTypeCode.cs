using System.ComponentModel;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel.Enums
{
    /// <summary>
    /// http://smartcityplatform.enea.it/specification/semantic/1.0/gc/SensorTypeCode.gc
    /// </summary>
    public enum SensorTypeCode
    {
        [Description("Smart Plug")]
        SmartPlug = 1,
        [Description("Smart Switch")]
        SmartSwitch = 2,
        [Description("Smart Presence")]
        SmartPresence = 3,
        [Description("Confort Multisensor")]
        ConfortMultisensor = 4,
        [Description("Weather Multisensor")]
        WeatherMultisensor = 5,
    }
}
