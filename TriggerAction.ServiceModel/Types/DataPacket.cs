using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DataPackets")]
    public partial class DataPacket : IHasId<int>
    {
        [Alias("PacketId")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? GatewayId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime? Timestamp { get; set; }
        [Required]
        public string RawData { get; set; }
        public int? SecondsFromAcquisition { get; set; }
    }

}

