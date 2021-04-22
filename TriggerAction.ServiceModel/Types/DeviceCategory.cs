using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DeviceCategories")]
    public class DeviceCategory
        : IHasId<int>
    {
        [AutoIncrement]
        [Alias("DeviceCategoryId")]
        public int Id { get; set; }
        public string Label { get; set; }
        [Compute]
        public DateTimeOffset CreatedUtc { get; set; }
        [Compute]
        public DateTimeOffset ModifiedUtc { get; set; }
    }
}
