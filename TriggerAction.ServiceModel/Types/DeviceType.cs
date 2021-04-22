using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DeviceTypes")]
    public class DeviceType
        : IHasId<int>
    {
        [AutoIncrement]
        [Alias("DeviceTypeId")]
        public int Id { get; set; }
        public string SKU { get; set; }
        public int XType { get; set; }
        public string Vendor { get; set; }
        public int Version { get; set; }
        public string UniqueId { get; set; }
        public string Label { get; set; }
        [References(typeof(DeviceCategory))]
        [Reference]
        public DeviceCategory DeviceCategory { get; set; }
        public int DeviceCategoryId { get; set; }
        [Compute]
        public DateTimeOffset CreatedUtc { get; set; }
        [Compute]
        public DateTimeOffset ModifiedUtc { get; set; }
        public int ProfileVersion { get; set; }
    }
}
