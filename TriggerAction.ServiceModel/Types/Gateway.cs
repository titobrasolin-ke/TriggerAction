using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("Gateways")]
    public class Gateway // Data Model
        : IHasId<int>
    {
        [AutoIncrement]
        [Alias("GatewayId")]
        public int Id { get; set; }
        [StringLength(32)]
        public string MACAddress { get; set; }
        public DateTime? InstallDate { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string Location { get; set; }
        public string LastIPAddress { get; set; }
        public int TotalConnection { get; set; }
        public int TotalDisconnection { get; set; }
        public DateTime LastConnectionDate { get; set; }
        public int? GatewayTypeId { get; set; }
        [StringLength(50)]
        public string IpAddress { get; set; }
        public int? IpPort { get; set; }
        public int? TimeoutMs { get; set; }

        [Reference]
        public List<Device> Devices { get; set; }

        public Gateway()
        {
            LastConnectionDate = new DateTime(1800, 1, 1);
        }
    }
}
