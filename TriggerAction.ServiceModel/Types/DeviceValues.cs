using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.Collections.Generic;

namespace TriggerAction.ServiceModel.Types
{
    [Alias("DeviceValues")]
    public partial class DeviceValue : IHasId<int>
    {
        [Alias("DeviceValueId")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        [Required]
        public string ValueUniqueId { get; set; }
        [Required]
        public decimal Offset { get; set; }
        [Required]
        public string Unit { get; set; }
        public decimal? TransformFactor { get; set; }
        public int? ValueTypeId { get; set; }
        [Required]
        public string BatchOperationType { get; set; }
        public string Label { get; set; }
    }

    public class CreateDeviceValue : ICreateDb<DeviceValue>, IReturn<CreateDeviceValueResponse>
    {
        public int? DeviceId { get; set; }
        public string ValueUniqueId { get; set; }
        public decimal Offset { get; set; }
        public string Unit { get; set; }
        public decimal? TransformFactor { get; set; }
        public int? ValueTypeId { get; set; }
        public string BatchOperationType { get; set; }
        public string Label { get; set; }
    }

    public class CreateDeviceValueResponse
    {
        public int Id { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class UpdateDeviceValue :
        IUpdateDb<DeviceValue>, IReturn<UpdateDeviceValueResponse>
    {
        public int Id { get; set; }
        public Dictionary<string,string> Location { get; set; }
    }

    public class UpdateDeviceValueResponse
    {
        public int Id { get; set; }
        public DeviceValue Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public partial class DeviceValueQuery : QueryDb<DeviceValue>
    {
        public int? Id { get; set; }

        public int? IdGreaterThanOrEqualTo { get; set; }
        public int? IdGreaterThan { get; set; }
        public int? IdLessThan { get; set; }
        public int? IdLessThanOrEqualTo { get; set; }
        public int? IdNotEqualTo { get; set; }
        public int[] IdBetween { get; set; }
        public int[] IdIn { get; set; }

        public int? DeviceId { get; set; }

        public int? DeviceIdGreaterThanOrEqualTo { get; set; }
        public int? DeviceIdGreaterThan { get; set; }
        public int? DeviceIdLessThan { get; set; }
        public int? DeviceIdLessThanOrEqualTo { get; set; }
        public int? DeviceIdNotEqualTo { get; set; }
        public int?[] DeviceIdBetween { get; set; }
        public int?[] DeviceIdIn { get; set; }

        public string ValueUniqueId { get; set; }

        public string ValueUniqueIdStartsWith { get; set; }
        public string ValueUniqueIdEndsWith { get; set; }
        public string ValueUniqueIdContains { get; set; }
        public string ValueUniqueIdLike { get; set; }
        public string[] ValueUniqueIdBetween { get; set; }
        public string[] ValueUniqueIdIn { get; set; }

        public decimal? Offset { get; set; }

        public decimal? OffsetGreaterThanOrEqualTo { get; set; }
        public decimal? OffsetGreaterThan { get; set; }
        public decimal? OffsetLessThan { get; set; }
        public decimal? OffsetLessThanOrEqualTo { get; set; }
        public decimal? OffsetNotEqualTo { get; set; }
        public decimal[] OffsetBetween { get; set; }
        public decimal[] OffsetIn { get; set; }

        public string Unit { get; set; }

        public string UnitStartsWith { get; set; }
        public string UnitEndsWith { get; set; }
        public string UnitContains { get; set; }
        public string UnitLike { get; set; }
        public string[] UnitBetween { get; set; }
        public string[] UnitIn { get; set; }

        public decimal? TransformFactor { get; set; }

        public decimal? TransformFactorGreaterThanOrEqualTo { get; set; }
        public decimal? TransformFactorGreaterThan { get; set; }
        public decimal? TransformFactorLessThan { get; set; }
        public decimal? TransformFactorLessThanOrEqualTo { get; set; }
        public decimal? TransformFactorNotEqualTo { get; set; }
        public decimal?[] TransformFactorBetween { get; set; }
        public decimal?[] TransformFactorIn { get; set; }

        public int? ValueTypeId { get; set; }

        public int? ValueTypeIdGreaterThanOrEqualTo { get; set; }
        public int? ValueTypeIdGreaterThan { get; set; }
        public int? ValueTypeIdLessThan { get; set; }
        public int? ValueTypeIdLessThanOrEqualTo { get; set; }
        public int? ValueTypeIdNotEqualTo { get; set; }
        public int?[] ValueTypeIdBetween { get; set; }
        public int?[] ValueTypeIdIn { get; set; }

        public string BatchOperationType { get; set; }

        public string BatchOperationTypeStartsWith { get; set; }
        public string BatchOperationTypeEndsWith { get; set; }
        public string BatchOperationTypeContains { get; set; }
        public string BatchOperationTypeLike { get; set; }
        public string[] BatchOperationTypeBetween { get; set; }
        public string[] BatchOperationTypeIn { get; set; }

        public string Label { get; set; }

        public string LabelStartsWith { get; set; }
        public string LabelEndsWith { get; set; }
        public string LabelContains { get; set; }
        public string LabelLike { get; set; }
        public string[] LabelBetween { get; set; }
        public string[] LabelIn { get; set; }

    }
}

