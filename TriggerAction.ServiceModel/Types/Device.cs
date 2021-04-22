using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("Devices")]
    public class Device // Data Model
        : IHasId<int>
    {
        [AutoIncrement]
        [Alias("DeviceId")]
        public int Id { get; set; }
        [Reference]
        public Gateway Gateway { get; set; }
        public int? GatewayId { get; set; }
        [Reference]
        public Plant Plant { get; set; }
        public int? PlantId { get; set; }
        public int LogicId { get; set; }
        public bool IsActive { get; set; }
        [Reference]
        public DeviceType DeviceType { get; set; }
        public int? DeviceTypeId { get; set; }
        public string SensorLabel { get; set; }
        public string UserLabel { get; set; }
        public string Location { get; set; }
        public int Slave { get; set; }
        public int? ViewId { get; set; }
        public bool IsDataExpected { get; set; }
        public bool IsMissing { get; set; }
        public bool IsLost { get; set; }
        public int MissingCount { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime UninstallDate { get; set; }
        public int? PollInterval { get; set; }
    }
}
