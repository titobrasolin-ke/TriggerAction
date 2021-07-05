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

        [Ignore, Description("Riferimento geografico: latitudine")]
        public double? Latitude { get => Location?.Latitude; set => Location.Latitude = value; }
        [Ignore, Description("Riferimento geografico: longitudine")]
        public double? Longitude { get => Location?.Longitude; set => Location.Longitude = value; }
        [Ignore, Description("Identificatore dell'edificio")]
        public string BuildingID { get => Location?.BuildingID; set => Location.BuildingID = value; }
        [Ignore, Description("Codice che identifica una stanza")]
        public string RoomID { get => Location?.RoomID; set => Location.RoomID = value; }
        [Ignore, Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RealEstateUnitID { get => Location?.RealEstateUnitID; set => Location.RealEstateUnitID = value; }
        [Ignore, Description("Tipologia di stanza in cui sensore è installato es. cucina, bagno, ufficio")]
        public string RoomCode { get => Location?.RoomCode; set => Location.RoomCode = value; }
        [Ignore, Description("Identificativo dell'area osservata.")]
        public string LocationID { get => Location?.LocationID; set => Location.LocationID = value; }
        [Ignore, Description("Uso finale dell'energia elettrica consumata (ad es. illuminazione, forza motrice, generale, condizionamento, ...)")]
        public string ElectricityEndUseCode { get => Location?.ElectricityEndUseCode; set => Location.ElectricityEndUseCode = value; }
        [Ignore, Description("tipologia di sensore che effettua la misura es. smart plug, smart switch, presence, ...")]
        public string SensorTypeCode { get => Location?.SensorTypeCode; set => Location.SensorTypeCode = value; }
        [Ignore, Description("Nome della stazione Meteo che ha fornito i dati")]
        public string MeteoStationName { get => Location?.MeteoStationName; set => Location.MeteoStationName = value; }
        [Ignore, Description("Nome della stazione di monitoraggio che ha fornito i dati")]
        public string MonitoringStationName { get => Location?.MonitoringStationName; set => Location.MonitoringStationName = value; }
        [Ignore, Description("Identificativo del sistema energetico (o macchina o apparecchio)")]
        public string EnergySystemID { get => Location?.EnergySystemID; set => Location.EnergySystemID = value; }
    }

    public class LocationInfo
    {
        [Description("Riferimento geografico: latitudine")]
        public double? Latitude { get; set; }
        [Description("Riferimento geografico: longitudine")]
        public double? Longitude { get; set; }
        [Description("Identificatore dell'edificio")]
        public string BuildingID { get; set; }
        [Description("Codice che identifica una stanza")]
        public string RoomID { get; set; }
        [Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RealEstateUnitID { get; set; }
        [Description("Tipologia di stanza in cui sensore è installato es. cucina, bagno, ufficio")]
        public string RoomCode { get; set; }
        [Description("Identificativo dell'area osservata.")]
        public string LocationID { get; set; }
        [Description("Uso finale dell'energia elettrica consumata (ad es. illuminazione, forza motrice, generale, condizionamento, ...)")]
        public string ElectricityEndUseCode { get; set; }
        [Description("tipologia di sensore che effettua la misura es. smart plug, smart switch, presence, ...")]
        public string SensorTypeCode { get; set; }
        [Description("Nome della stazione Meteo che ha fornito i dati")]
        public string MeteoStationName { get; set; }
        [Description("Nome della stazione di monitoraggio che ha fornito i dati")]
        public string MonitoringStationName { get; set; }
        [Description("Identificativo del sistema energetico (o macchina o apparecchio)")]
        public string EnergySystemID { get; set; }
    }

    [AutoQueryViewer(DefaultFields = "Id,SensorLabel,UserLabel,Location,DeviceTypeId,LogicId,Slave,GatewayId,PlantId")]
    public partial class DeviceQuery : QueryDb<Device> { }

    public class UpdateDevice :
        IUpdateDb<Device>, IReturn<UpdateDeviceResponse>
    {
        public int Id { get; set; }
        public string SensorLabel { get; set; }
        public string UserLabel { get; set; }
        [Description("Riferimento geografico: latitudine")]
        public double? Latitude { get; set; }
        [Description("Riferimento geografico: longitudine")]
        public double? Longitude { get; set; }
        [Description("Identificatore dell'edificio")]
        public string BuildingID { get; set; }
        [Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RoomID { get; set; }
        [Description("Identificativo dell'unità immobiliare (es. appartemento, edificio, abitazione indipendente...)")]
        public string RealEstateUnitID { get; set; }
        [Description("Tipologia di stanza in cui sensore è installato es. cucina, bagno, ufficio")]
        public string RoomCode { get; set; }
        [Description("Identificativo dell'area osservata.")]
        public string LocationID { get; set; }
        [Description("Uso finale dell'energia elettrica consumata (ad es. illuminazione, forza motrice, generale, condizionamento, ...)")]
        public string ElectricityEndUseCode { get; set; }
        [Description("tipologia di sensore che effettua la misura es. smart plug, smart switch, presence, ...")]
        public string SensorTypeCode { get; set; }
        [Description("Nome della stazione Meteo che ha fornito i dati")]
        public string MeteoStationName { get; set; }
        [Description("Nome della stazione di monitoraggio che ha fornito i dati")]
        public string MonitoringStationName { get; set; }
        [Description("Identificativo del sistema energetico (o macchina o apparecchio)")]
        public string EnergySystemID { get; set; }
    }

    public class UpdateDeviceResponse
    {
        public int Id { get; set; }
        public Device Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
