using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.Collections.Generic;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("Plants")]
    public class Plant // Data model
        : IHasId<int>
    {
        [AutoIncrement]
        [Alias("PlantId")]
        public int Id { get; set; }
        public string Label { get; set; }
        // public string ImagePath_ { get; set; }

        [Reference]
        public List<Device> Devices { get; set; }
    }
}
