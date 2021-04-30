using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("ValueTypes")]
    public partial class ValueType : IHasId<int>
    {
        [Alias("ValueTypeId")]
        [AutoIncrement]
        public int Id { get; set; }
        public string Unit { get; set; }
        public decimal? TransformFactor { get; set; }
        public string Label { get; set; }
        public string ShortLabel { get; set; }
        public bool? IsCounter { get; set; }
        [Required]
        public int ModBUSStartRegister { get; set; }
        [Required]
        public string DisplayFormat { get; set; }
        [Required]
        public int ModBusDataType { get; set; }
        public string UniqueId { get; set; }
        public int? ValueTypeCategoryId { get; set; }
        public byte? ExcelFormat { get; set; }
        [Required]
        public DateTime CreatedUtc { get; set; }
        [Required]
        public DateTime ModifiedUtc { get; set; }
        public decimal? Offset { get; set; }
        public string Key { get; set; }
    }

}
