using ServiceStack;
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
        public string SensorLabel { get; set; }
        public string UserLabel { get; set; }
        public LocationInfo Location { get; set; }
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
        public int Slave { get; set; }
        public int? ViewId { get; set; }
        public bool IsDataExpected { get; set; }
        public bool IsMissing { get; set; }
        public bool IsLost { get; set; }
        public int MissingCount { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime UninstallDate { get; set; }
        public int? PollInterval { get; set; }

        [Ignore]
        public double? Latitude { get => Location?.Latitude; set => Location.Latitude = value; }
        [Ignore]
        public double? Longitude { get => Location?.Longitude; set => Location.Longitude = value; }
        [Ignore]
        public string RealEstateUnitID { get => Location?.RealEstateUnitID; set => Location.RealEstateUnitID = value; }
        [Ignore]
        public string RoomCode { get => Location?.RoomCode; set => Location.RoomCode = value; }
        [Ignore]
        public string LocationID { get => Location?.LocationID; set => Location.LocationID = value; }
        [Ignore]
        public string PDRID { get => Location?.PDRID; set => Location.PDRID = value; }
        [Ignore]
        public string ElectricityEndUseCode { get => Location?.ElectricityEndUseCode; set => Location.ElectricityEndUseCode = value; }
    }

    public class LocationInfo
    {
        [Description("Riferimento geografico: latitudine")]
        public double? Latitude { get; set; }
        [Description("Riferimento geografico: longitudine")]
        public double? Longitude { get; set; }
        [Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RealEstateUnitID { get; set; }
        [Description("Tipologia di stanza in cui sensore è installato es. cucina, bagno, ufficio")]
        public string RoomCode { get; set; }
        [Description("Identificativo dell'area osservata.")]
        public string LocationID { get; set; }
        [Description("Codice PDR cui è associata l'utenza")]
        public string PDRID { get; set; }
        [Description("Uso finale dell'energia elettrica consumata (ad es. illuminazione, forza motrice, generale, condizionamento, ...)")]
        public string ElectricityEndUseCode { get; set; }
    }

    [AutoQueryViewer(DefaultFields = "Id,SensorLabel,UserLabel,Location,DeviceTypeId,LogicId,Slave,GatewayId,PlantId")]
    public partial class DeviceQuery : QueryDb<Device> { }

    public class UpdateDevice :
        IUpdateDb<Device>, IReturn<UpdateDeviceResponse>
    {
        public int Id { get; set; }
        public string SensorLabel { get; set; }
        public string UserLabel { get; set; }
        [Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RealEstateUnitID { get; set; }
        [Description("Tipologia di stanza in cui sensore è installato es. cucina, bagno, ufficio")]
        public int? RoomCode { get; set; }
        [Description("Identificativo dell'area osservata.")]
        public string LocationID { get; set; }
        [Description("Codice PDR cui è associata l'utenza")]
        public string PDRID { get; set; }
    }

    public class UpdateDeviceResponse
    {
        public int Id { get; set; }
        public Device Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
