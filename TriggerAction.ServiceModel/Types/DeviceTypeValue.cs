using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DeviceTypeValues")]
    public partial class DeviceTypeValue : IHasId<int>
    {
        [Alias("DeviceTypeValueId")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? ValueTypeId { get; set; }
    }

}
