using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DataValues")]
    public class DataValue // Data Model
        : IHasId<long>
    {
        [AutoIncrement]
        [Alias("DataValueId")]
        public long Id { get; set; }
        public long PacketId { get; set; }
        public decimal Value { get; set; }
        public decimal RawValue { get; set; }
        public decimal Offset { get; set; }
        public decimal TransformFactor { get; set; }
        public string Unit { get; set; }
        public int ValueTypeId { get; set; }
        public int Error { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
