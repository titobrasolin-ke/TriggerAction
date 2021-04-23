using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TriggerAction.ServiceModel.Types
{
    [DataContract]
    public partial class Template
    {
        [DataMember(Name="UrbanDataset")]
        public UrbanDataset UrbanDataset { get; set; }
    }

    [DataContract]
    public partial class UrbanDataset
    {
        /// <value>
        /// The <c>Specification</c> section contains the information that describes the UrbanDataset, such as: the
        /// version of the specifications and the list of properties that are represented by it.
        /// See <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#specificationsection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="specification")]
        public Specification Specification { get; set; }

        /// <value>
        /// The <c>Context</c> section allows you to contextualize the UrbanDataset (for example via a time stamp and
        /// geographic coordinates).
        /// See: <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#contextsection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="context")]
        public Context Context { get; set; }

        /// <value>
        /// The <c>Values</c> section is composed of one or more rows (1 row = 1 group of properties), each of which
        /// contains the name and value of one of the data that is represented by the UrbanDataset.
        /// See: <a href="http://smartcityplatform.enea.it/#/en/specification/information/2.0/index.html#valuessection">SCPS Information 2.0</a>.
        /// </value>
        [DataMember(Name="values")]
        public Values Values { get; set; }
    }

    [DataContract]
    public partial class Context
    {
        [DataMember(Name="producer")]
        public Producer Producer { get; set; }

        [DataMember(Name="timeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name="timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name="coordinates")]
        public Coordinates Coordinates { get; set; }

        [DataMember(Name="language")]
        public string Language { get; set; }
    }

    [DataContract]
    public partial class Coordinates
    {
        [DataMember(Name="format")]
        public string Format { get; set; }

        [DataMember(Name="latitude")]
        public double Latitude { get; set; }

        [DataMember(Name="longitude")]
        public double Longitude { get; set; }

        [DataMember(Name="height")]
        public double? Height { get; set; }
    }

    [DataContract]
    public partial class Producer
    {
        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="schemeID")]
        public string SchemeId { get; set; }
    }

    [DataContract]
    public partial class Specification
    {
        [DataMember(Name="version")]
        public string Version { get; set; }

        [DataMember(Name="id")]
        public Id Id { get; set; }

        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="uri")]
        public Uri Uri { get; set; }

        [DataMember(Name="properties")]
        public Properties Properties { get; set; }
    }

    [DataContract]
    public partial class Id
    {
        [DataMember(Name="value")]
        public string Value { get; set; }

        [DataMember(Name="schemeID")]
        public string SchemeId { get; set; }
    }

    [DataContract]
    public partial class Properties
    {
        [DataMember(Name="propertyDefinition")]
        public List<PropertyDefinition> PropertyDefinition { get; set; }
    }

    [DataContract]
    public partial class PropertyDefinition
    {
        public class CommonPropertyNames
        {
            [Description("Energia attiva consumata (kilowattHour, double)")]
            public static readonly string ActiveEnergy = "ActiveEnergy";
            [Description("Potenza attiva fase (kilowatt, double)")]
            public static readonly string ActivePower = "ActivePower";
            [Description("Potenza attiva fase (kilowatt, double)")]
            public static readonly string ActivePowerPhase = "ActivePowerPhase";
            [Description("Potenza attiva fase R (kilowatt, double)")]
            public static readonly string ActivePowerPhase1 = "ActivePowerPhase1";
            [Description("Potenza attiva fase S (kilowatt, double)")]
            public static readonly string ActivePowerPhase2 = "ActivePowerPhase2";
            [Description("Potenza attiva fase T (kilowatt, double)")]
            public static readonly string ActivePowerPhase3 = "ActivePowerPhase3";
            [Description("indirizzo senza numero civico (dimensionless, string)")]
            public static readonly string Address = "Address";
            [Description("numero civico di un indirizzo es. 8, 7a, snc (dimensionless, string)")]
            public static readonly string AddressNumber = "AddressNumber";
            [Description("Fascia di et� (dimensionless, string)")]
            public static readonly string AgeRange = "AgeRange";
            [Description("Temperatura dell'aria (degreeCelsius, double)")]
            public static readonly string AirTemperature = "AirTemperature";
            [Description("Potenza apparente (kilovoltampere, double)")]
            public static readonly string ApparentPower = "ApparentPower";
            [Description("Potenza apparente (kilovoltampere, double)")]
            public static readonly string ApparentPowerPhase = "ApparentPowerPhase";
            [Description("Potenza apparente R (kilovoltampere, double)")]
            public static readonly string ApparentPowerPhase1 = "ApparentPowerPhase1";
            [Description("Potenza apparente S (kilovoltampere, double)")]
            public static readonly string ApparentPowerPhase2 = "ApparentPowerPhase2";
            [Description("Potenza apparente T (kilovoltampere, double)")]
            public static readonly string ApparentPowerPhase3 = "ApparentPowerPhase3";
            [Description("tipo di appliance associata alla smart plug/switch es. lavatrice, televisione, lampada (dimensionless, string)")]
            public static readonly string ApplianceCode = "ApplianceCode";
            [Description("vincolo architettonico : si / no / non so (dimensionless, string)")]
            public static readonly string ArchitecturalConstraint = "ArchitecturalConstraint";
            [Description("Superficie (squareMetre, double)")]
            public static readonly string Area = "Area";
            [Description("Concentrazione di Arsenico (As) (microgramPerCubicMeter, double)")]
            public static readonly string ArsenicConcentration = "ArsenicConcentration";
            [Description("Pressione atmosferica osservata (hectopascal, double)")]
            public static readonly string AtmosphericPressur = "AtmosphericPressur";
            [Description("Pressione atmosferica osservata (hectopascal, double)")]
            public static readonly string AtmosphericPressure = "AtmosphericPressure";
            [Description("Concentrazione di Benzoapirene (BaP) (microgramPerCubicMeter, double)")]
            public static readonly string BenzoapyreneConcentration = "BenzoapyreneConcentration";
            [Description("Codice identificativo dell'anomalia dell'edificio. (dimensionless, string)")]
            public static readonly string BuildingEnergyAnomalyCode = "BuildingEnergyAnomalyCode";
            [Description("Numero di anomalie con priorit� alta, riscontrate in un edificio (dimensionless, integer)")]
            public static readonly string BuildingEnergyHighAnomalies = "BuildingEnergyHighAnomalies";
            [Description("Numero di anomalie con priorit� bassa, riscontrate in un edificio (dimensionless, integer)")]
            public static readonly string BuildingEnergyLowAnomalies = "BuildingEnergyLowAnomalies";
            [Description("Numero di anomalie con priorit� media, riscontrate in un edificio (dimensionless, integer)")]
            public static readonly string BuildingEnergyMeanAnomalies = "BuildingEnergyMeanAnomalies";
            [Description("Identificatore dell'edificio (dimensionless, string)")]
            public static readonly string BuildingID = "BuildingID";
            [Description("Etichetta associata all'edificio (dimensionless, string)")]
            public static readonly string BuildingName = "BuildingName";
            [Description("periodo di costruzione di un edificio es. 1981-1992 (dimensionless, string)")]
            public static readonly string BuildingPeriodCode = "BuildingPeriodCode";
            [Description("tipologia di edificio es. : residenziale, scuola, caserma, ospedale, ... (dimensionless, string)")]
            public static readonly string BuildingTypeCode = "BuildingTypeCode";
            [Description("Benzene (microgramPerCubicMeter, double)")]
            public static readonly string C6H6 = "C6H6";
            [Description("dato catastale immobile : particella (dimensionless, string)")]
            public static readonly string CadastralParcel = "CadastralParcel";
            [Description("dato catastale immobile : foglio (dimensionless, string)")]
            public static readonly string CadastralSheet = "CadastralSheet";
            [Description("dato catastale immobile : subalterno (dimensionless, string)")]
            public static readonly string CadastralSub = "CadastralSub";
            [Description("Concentrazione di Cadmio (Cd) (microgramPerCubicMeter, double)")]
            public static readonly string CadmiumConcentration = "CadmiumConcentration";
            [Description("Indicatore urbano su illuminazione pubblica (dimensionless, string)")]
            public static readonly string CityPLIndicatorCode = "CityPLIndicatorCode";
            [Description("Monossido di carbonio (microgramPerCubicMeter, double)")]
            public static readonly string CO = "CO";
            [Description("Biossido di carbonio (partsPerMillion, double)")]
            public static readonly string CO2 = "CO2";
            [Description("Pattern di comunicazione (valori possibili: \"Push\", \"Request Response\", \"Pubblish Subscribe\") (dimensionless, enumeration)")]
            public static readonly string CommunicationPattern = "CommunicationPattern";
            [Description("anno di costruzione di un edificio (se noto) (dimensionless, integer)")]
            public static readonly string ConstructionYear = "ConstructionYear";
            [Description("Riferimento geografico")]
            public static readonly string coordinates = "coordinates";
            [Description("Corrente della linea (ampere, double)")]
            public static readonly string CurrentLine = "CurrentLine";
            [Description("Corrente della linea 1 (ampere, double)")]
            public static readonly string CurrentLine1 = "CurrentLine1";
            [Description("Corrente della linea 2 (ampere, double)")]
            public static readonly string CurrentLine2 = "CurrentLine2";
            [Description("Corrente della linea 3 (ampere, double)")]
            public static readonly string CurrentLine3 = "CurrentLine3";
            [Description("Descrizione testuale dei dati inviati (dimensionless, string)")]
            public static readonly string DataDescription = "DataDescription";
            [Description("URL dello Schema del Formato con cui si invieranno i dati (dimensionless, string)")]
            public static readonly string DataFormat = "DataFormat";
            [Description("Codice identificativo dell'anomalia del device (dimensionless, string)")]
            public static readonly string DeviceAnomalyCode = "DeviceAnomalyCode";
            [Description("Identificatore del device (dimensionless, string)")]
            public static readonly string DeviceID = "DeviceID";
            [Description("Numero totale delle anomalie individuate su un device (dimensionless, integer)")]
            public static readonly string DeviceTotalAnomalies = "DeviceTotalAnomalies";
            [Description("Punto di rugiada (degreeCelsius, double)")]
            public static readonly string DewPoint = "DewPoint";
            [Description("misura della radiazione solare diretta (wattPerSquareMetre, double)")]
            public static readonly string DirectSolarRadition = "DirectSolarRadition";
            [Description("luminosit� verticale direzione est (lux, double)")]
            public static readonly string EastLuminosity = "EastLuminosity";
            [Description("Consumo energia elettrica (kilowattHour, double)")]
            public static readonly string ElectricConsumption = "ElectricConsumption";
            [Description("Uso finale dell'energia elettrica consumata (ad es. illuminazione, forza motrice, generale, condizionamento,�) (dimensionless, string)")]
            public static readonly string ElectricityUseCode = "ElectricityUseCode";
            [Description("Identificativo del quadro elettrico (dimensionless, string)")]
            public static readonly string ElectricPanelID = "ElectricPanelID";
            [Description("Energia elettrica prodotta (kilowattHour, double)")]
            public static readonly string ElectricProduction = "ElectricProduction";
            [Description("Ora di fine di una fascia oraria (dimensionless, Time)")]
            public static readonly string EndTimeSlot = "EndTimeSlot";
            [Description("Marca temporale indicante la fine del periodo (dimensionless, dateTime)")]
            public static readonly string end_ts = "end_ts";
            [Description("Identificativo del sistema energetico (o macchina o apparecchio) (dimensionless, string)")]
            public static readonly string EnergySystemID = "EnergySystemID";
            [Description("Potenza del sistema (kilowatt, double)")]
            public static readonly string EnergySystemPower = "EnergySystemPower";
            [Description("Nome che identifica la tecnologia del sistema energetico (dimensionless, string)")]
            public static readonly string EnergySystemTechnologyName = "EnergySystemTechnologyName";
            [Description("Valutazione del valore del KPI/indicatore es. 'ok�, �fuori soglia�, �non disponibile� (dimensionless, string)")]
            public static readonly string EvaluationIndicator = "EvaluationIndicator";
            [Description("Superficie calpestabile (squareMetre, double)")]
            public static readonly string FloorArea = "FloorArea";
            [Description("Identificazione del piano specifico di un edificio (dimensionless, integer)")]
            public static readonly string FloorNumber = "FloorNumber";
            [Description("Formato wgs84 in cui sono espresse le coordinate (attributo opzionale) (dimensionless, string)")]
            public static readonly string format = "format";
            [Description("Consumo gas (cubicMetre, double)")]
            public static readonly string GasConsumption = "GasConsumption";
            [Description("Genere (dimensionless, string)")]
            public static readonly string GenderCode = "GenderCode";
            [Description("Identificatore del Gruppo del social network (dimensionless, string)")]
            public static readonly string GroupID = "GroupID";
            [Description("Nome del Gruppo del social network (dimensionless, string)")]
            public static readonly string GroupName = "GroupName";
            [Description("Link (url) al Gruppo del social network (dimensionless, string)")]
            public static readonly string GroupURL = "GroupURL";
            [Description("Altitudine (proprieta' opzionale) (dimensionless, double)")]
            public static readonly string height = "height";
            [Description("misura della luminosit� orizzontale (lux, double)")]
            public static readonly string HorizontalLuminosity = "HorizontalLuminosity";
            [Description("Inclinazione sul piano orizzontale (degree, double)")]
            public static readonly string HorizontalTilt = "HorizontalTilt";
            [Description("Valore calcolato per l'indicatore (dimensionless, double)")]
            public static readonly string IndicatorValue = "IndicatorValue";
            [Description("Indice di Qualit� dell'Aria Indoor (dimensionless, double)")]
            public static readonly string IndoorAirQualityIndex = "IndoorAirQualityIndex";
            [Description("Misura (diretta o stimata) della CO2 in ambiente chiuso (partsPerMillion, double)")]
            public static readonly string IndoorCO2 = "IndoorCO2";
            [Description("misura (indiretta) della CO2 correlata al VOC equivalente riferito al respiro medio di un essere umano (partsPerMillion, double)")]
            public static readonly string IndoorCO2Equivalent = "IndoorCO2Equivalent";
            [Description("Potenza frigorifera in input (kilowatt, double)")]
            public static readonly string InputCoolingPower = "InputCoolingPower";
            [Description("Potenza elettrica in input (kilowatt, double)")]
            public static readonly string InputElectricPower = "InputElectricPower";
            [Description("Potenza combustibile in input (kilowatt, double)")]
            public static readonly string InputFuelPower = "InputFuelPower";
            [Description("Potenza termica in input (kilowatt, double)")]
            public static readonly string InputThermalPower = "InputThermalPower";
            [Description("Valore calcolato per il KPI specifico (dimensionless, double)")]
            public static readonly string KPIValue = "KPIValue";
            [Description("vincolo paesaggistico : si / no / non so (dimensionless, string)")]
            public static readonly string LandscapeConstraint = "LandscapeConstraint";
            [Description("Latitudine (dimensionless, double)")]
            public static readonly string latitude = "latitude";
            [Description("Corrente della linea (ampere, double)")]
            public static readonly string LineCurrent = "LineCurrent";
            [Description("Identificativo dell'area osservata. (dimensionless, string)")]
            public static readonly string LocationID = "LocationID";
            [Description("Denominazione luogo (dimensionless, string)")]
            public static readonly string LocationName = "LocationName";
            [Description("Codice che indica lo stato dell'\"oggetto\" monitorato (dimensionless, string)")]
            public static readonly string LogCode = "LogCode";
            [Description("Descrizione testuale dello stato dell'oggetto monitorato (dimensionless, string)")]
            public static readonly string LogDescription = "LogDescription";
            [Description("Longitudine (dimensionless, double)")]
            public static readonly string longitude = "longitude";
            [Description("misura della luminosit� in lux (lux, double)")]
            public static readonly string Luminosity = "Luminosity";
            [Description("Numero di iscritti al Gruppo (dimensionless, integer)")]
            public static readonly string MemberCount = "MemberCount";
            [Description("Nome della stazione Meteo che ha fornito i dati (dimensionless, string)")]
            public static readonly string MeteoStationName = "MeteoStationName";
            [Description("Identificativo univoco del contatore (dimensionless, string)")]
            public static readonly string MeterID = "MeterID";
            [Description("Identificativo dello schema di codifica a cui appartiene l'identificativo fornito nel campo MeterID (dimensionless, string)")]
            public static readonly string MeterSchemeID = "MeterSchemeID";
            [Description("Input frigo minimo (kilowatt, double)")]
            public static readonly string MinInputCoolingPower = "MinInputCoolingPower";
            [Description("Input elettrico minimo (kilowatt, double)")]
            public static readonly string MinInputElectricPower = "MinInputElectricPower";
            [Description("Input combustibile minimo (kilowatt, double)")]
            public static readonly string MinInputFuelPower = "MinInputFuelPower";
            [Description("Input termico minimo (kilowatt, double)")]
            public static readonly string MinInputThermalPower = "MinInputThermalPower";
            [Description("Output frigo a carico minimo (kilowatt, double)")]
            public static readonly string MinOutputCoolingPower = "MinOutputCoolingPower";
            [Description("Output elettrico a carico minimo (kilowatt, double)")]
            public static readonly string MinOutputElectricPower = "MinOutputElectricPower";
            [Description("Output combustibile a carico minimo (kilowatt, double)")]
            public static readonly string MinOutputFuelPower = "MinOutputFuelPower";
            [Description("Output temico a carico minimo (kilowatt, double)")]
            public static readonly string MinOutputThermalPower = "MinOutputThermalPower";
            [Description("Nome della stazione di monitoraggio che ha fornito i dati (dimensionless, string)")]
            public static readonly string MonitoringStationName = "MonitoringStationName";
            [Description("Nazionalit� (dimensionless, string)")]
            public static readonly string Nationality = "Nationality";
            [Description("Concentrazione di Nichel (Ni) (microgramPerCubicMeter, double)")]
            public static readonly string NickelConcentration = "NickelConcentration";
            [Description("Monossido di azoto (microgramPerCubicMeter, double)")]
            public static readonly string NO = "NO";
            [Description("Biossido di azoto (microgramPerCubicMeter, double)")]
            public static readonly string NO2 = "NO2";
            [Description("Efficienza nominale del sistema (dimensionless, double)")]
            public static readonly string NominalEfficiency = "NominalEfficiency";
            [Description("Input frigo nominale (kilowatt, double)")]
            public static readonly string NominalInputCoolingPower = "NominalInputCoolingPower";
            [Description("Input elettrico nominale (kilowatt, double)")]
            public static readonly string NominalInputElectricPower = "NominalInputElectricPower";
            [Description("Input combustibile nominale (kilowatt, double)")]
            public static readonly string NominalInputFuelPower = "NominalInputFuelPower";
            [Description("Input termico nominale (kilowatt, double)")]
            public static readonly string NominalInputThermalPower = "NominalInputThermalPower";
            [Description("Temperatura nominale di lavoro raggiunta dal pannello fotovoltaico (NOCT) (degreeCelsius, double)")]
            public static readonly string NominalOperatingCellTemperature = "NominalOperatingCellTemperature";
            [Description("Output frigo a carico nominale (kilowatt, double)")]
            public static readonly string NominalOutputCoolingPower = "NominalOutputCoolingPower";
            [Description("Output elettrico a carico nominale (kilowatt, double)")]
            public static readonly string NominalOutputElectricPower = "NominalOutputElectricPower";
            [Description("Output combustibile a carico nominale (kilowatt, double)")]
            public static readonly string NominalOutputFuelPower = "NominalOutputFuelPower";
            [Description("Output termico a carico nominale (kilowatt, double)")]
            public static readonly string NominalOutputThermalPower = "NominalOutputThermalPower";
            [Description("luminosit� verticale direzione nord (lux, double)")]
            public static readonly string NorthLuminosity = "NorthLuminosity";
            [Description("Ozono (microgramPerCubicMeter, double)")]
            public static readonly string O3 = "O3";
            [Description("Numero di Occupanti di un ambiente confinato (dimensionless, integer)")]
            public static readonly string OccupantCount = "OccupantCount";
            [Description("Quantita' di pioggia calcolata in un'ora dai pluviometri in millimetri di accumulo (millimetre, double)")]
            public static readonly string OneHourRainfall = "OneHourRainfall";
            [Description("Costi operativi riferiti al periodo indicato (euro, double)")]
            public static readonly string OperatingCost = "OperatingCost";
            [Description("Costo di Operation and Maintenance (come proporzione rispetto alla potenza). L'unita' di misura e': Euro/kW (euro, double)")]
            public static readonly string OperationMaintenanceCost = "OperationMaintenanceCost";
            [Description("Potenza frigorifera in uscita (kilowatt, double)")]
            public static readonly string OutputCoolingPower = "OutputCoolingPower";
            [Description("Potenza elettrica in uscita (kilowatt, double)")]
            public static readonly string OutputElectricPower = "OutputElectricPower";
            [Description("Output combustibile (kilowatt, double)")]
            public static readonly string OutputFuelPower = "OutputFuelPower";
            [Description("Potenza termica in uscita (kilowatt, double)")]
            public static readonly string OutputThermalPower = "OutputThermalPower";
            [Description("Identificatore della Pagina del social network (dimensionless, string)")]
            public static readonly string PageID = "PageID";
            [Description("Numero di \"Like\" ricevuti dalla pagina (dimensionless, integer)")]
            public static readonly string PageLikeCount = "PageLikeCount";
            [Description("Nome della Pagina del social network (dimensionless, string)")]
            public static readonly string PageName = "PageName";
            [Description("Link (url) alla Pagina del social network (dimensionless, string)")]
            public static readonly string PageURL = "PageURL";
            [Description("Indica se il numero di presenze indicato � un parziale (true) o un totale (false) per il periodo indicato. (dimensionless, string)")]
            public static readonly string PartialCountFlag = "PartialCountFlag";
            [Description("Codice PDR cui � associata l'utenza (dimensionless, string)")]
            public static readonly string PDRID = "PDRID";
            [Description("KPI tecnici specifici del PELL IP (dimensionless, string)")]
            public static readonly string PellKPICode = "PellKPICode";
            [Description("Periodo durante il quale sono stati rilevati i dati riportati nella riga")]
            public static readonly string period = "period";
            [Description("Periodo espresso come valore. Alcuni esempi: - 2020, 2019, � , se PeriodValueQualifier = year - 1, 2, 30,� , se PeriodValueQualifier = week - Mon, Tue, Sat,� , se PeriodValueQualifier = day (dimensionless, string)")]
            public static readonly string PeriodValue = "PeriodValue";
            [Description("Qualificatore per l'elemento PeriodValue (dimensionless, string)")]
            public static readonly string PeriodValueQualifierCode = "PeriodValueQualifierCode";
            [Description("Tensione della fase (volt, double)")]
            public static readonly string PhaseVoltage = "PhaseVoltage";
            [Description("Indica la categoria (persone, veicoli) alla quale si riferisce il conteggio (dimensionless, string)")]
            public static readonly string PhysicalUnitCategoryCode = "PhysicalUnitCategoryCode";
            [Description("Identificatore dell'impianto (dimensionless, string)")]
            public static readonly string PlantID = "PlantID";
            [Description("Nome dell'impianto (dimensionless, string)")]
            public static readonly string PlantName = "PlantName";
            [Description("Identificatore della piattaforma software (dimensionless, string)")]
            public static readonly string PlatformID = "PlatformID";
            [Description("Nome della piattaforma software (dimensionless, string)")]
            public static readonly string PlatformName = "PlatformName";
            [Description("Identificatore della zona omogenea secondo criteri illuminotecnici di un area urbana con illuminazione pubblica (dimensionless, string)")]
            public static readonly string PLHomogeneousAreaID = "PLHomogeneousAreaID";
            [Description("quantit� di particolato, polveri sottili, con diametro <= 2,5 micron (microgramPerCubicMeter, double)")]
            public static readonly string PM2 = "PM2";
            [Description("quantit� di particolato, polveri sottili, con diametro <= 10 micron (microgramPerCubicMeter, double)")]
            public static readonly string PM10 = "PM10";
            [Description("indice PMV (predicted mean vote) della misura del confort indoor secondo la scala [-3,+3] (dimensionless, double)")]
            public static readonly string PMV = "PMV";
            [Description("Codice POD che identifica univocamente il punto di prelievo (dimensionless, string)")]
            public static readonly string PODID = "PODID";
            [Description("Numero di reazioni ai post pubblicati. (dimensionless, integer)")]
            public static readonly string PostReactionCount = "PostReactionCount";
            [Description("Numero di risposte ai post pubblicati. (dimensionless, integer)")]
            public static readonly string PostReplyCount = "PostReplyCount";
            [Description("Numero di utenti che hanno visualizzato i post della pagina (dimensionless, integer)")]
            public static readonly string PostVisualizationCount = "PostVisualizationCount";
            [Description("Fattore di potenza (dimensionless, double)")]
            public static readonly string PowerFactor = "PowerFactor";
            [Description("Fattore di potenza (dimensionless, double)")]
            public static readonly string PowerFactorPhase = "PowerFactorPhase";
            [Description("Fattore di potenza R (dimensionless, double)")]
            public static readonly string PowerFactorPhase1 = "PowerFactorPhase1";
            [Description("Fattore di potenza S (dimensionless, double)")]
            public static readonly string PowerFactorPhase2 = "PowerFactorPhase2";
            [Description("Fattore di potenza T (dimensionless, double)")]
            public static readonly string PowerFactorPhase3 = "PowerFactorPhase3";
            [Description("Probabilit�, espressa in percentuale, con cui si verificheranno delle precipitazioni. (dimensionless, double)")]
            public static readonly string PrecipitationProbabilityPercentage = "PrecipitationProbabilityPercentage";
            [Description("Numero di presenze rilevato (dimensionless, integer)")]
            public static readonly string PresenceCount = "PresenceCount";
            [Description("Percentuale di tempo in cui un ambiente ha avuto la presenza di almeno una pesona (dimensionless, double)")]
            public static readonly string PresencePercentage = "PresencePercentage";
            [Description("Numero di dispositivi di localizzazione (dimensionless, integer)")]
            public static readonly string PresenceScannerCount = "PresenceScannerCount";
            [Description("Codice Provincia (dimensionless, string)")]
            public static readonly string ProvinceCode = "ProvinceCode";
            [Description("Concentrazione di Radon (becquerelPerCubicMetre, double)")]
            public static readonly string RadonConcentration = "RadonConcentration";
            [Description("Energia reattiva (inductiveEnergy + capacitiveEnergy) (kilovoltamperereactivePerHour, double)")]
            public static readonly string ReactiveEnergy = "ReactiveEnergy";
            [Description("Potenza reattiva (kilovoltamperereactive, double)")]
            public static readonly string ReactivePower = "ReactivePower";
            [Description("Potenza reattiva (kilovoltamperereactive, double)")]
            public static readonly string ReactivePowerPhase = "ReactivePowerPhase";
            [Description("Potenza reattiva R (kilovoltamperereactive, double)")]
            public static readonly string ReactivePowerPhase1 = "ReactivePowerPhase1";
            [Description("Potenza reattiva S (kilovoltamperereactive, double)")]
            public static readonly string ReactivePowerPhase2 = "ReactivePowerPhase2";
            [Description("Potenza reattiva T (kilovoltamperereactive, double)")]
            public static readonly string ReactivePowerPhase3 = "ReactivePowerPhase3";
            [Description("Identificativo dell'unit� immobiliare (es. appartemento, edificio, abitazione indipendente...) (dimensionless, string)")]
            public static readonly string RealEstateUnitID = "RealEstateUnitID";
            [Description("numero di unit� immobiliari (dimensionless, integer)")]
            public static readonly string RealEstateUnits = "RealEstateUnits";
            [Description("Codice Regione (dimensionless, string)")]
            public static readonly string RegionCode = "RegionCode";
            [Description("Umidit� relativa dell'aria espressa in percentuale. (dimensionless, double)")]
            public static readonly string RelativeHumidity = "RelativeHumidity";
            [Description("Tipologia di stanza in cui sensore � installato es. cucina, bagno, ufficio (dimensionless, string)")]
            public static readonly string RoomCode = "RoomCode";
            [Description("Identificativo di uno schema di identificazione utilizzato (dimensionless, string)")]
            public static readonly string SchemeID = "SchemeID";
            [Description("identificativo univoco del sensore es. serial number, mac address (dimensionless, string)")]
            public static readonly string SensorID = "SensorID";
            [Description("marca/modello (codice) del sensore (dimensionless, string)")]
            public static readonly string SensorName = "SensorName";
            [Description("tipologia di sensore che effettua la misura es. smart plug, smart switch, presence, ... (dimensionless, string)")]
            public static readonly string SensorTypeCode = "SensorTypeCode";
            [Description("Concentrazione di biossido di Zolfo (microgramPerCubicMeter, double)")]
            public static readonly string SO2 = "SO2";
            [Description("Nome o identificatore del social network a cui sono riferiti i dati (dimensionless, string)")]
            public static readonly string SocialNetworkName = "SocialNetworkName";
            [Description("Radiazione solare totale osservata misurata in Watt per metro quadrato (wattPerSquareMetre, double)")]
            public static readonly string SolarRadiation = "SolarRadiation";
            [Description("Orientamento rispetto al sud (degree, double)")]
            public static readonly string SouthDirection = "SouthDirection";
            [Description("luminosit� verticale direzione sud (lux, double)")]
            public static readonly string SouthLuminosity = "SouthLuminosity";
            [Description("Ora di inizio di una fascia oraria (dimensionless, Time)")]
            public static readonly string StartTimeSlot = "StartTimeSlot";
            [Description("Marca temporale indicante l'inizio del periodo (dimensionless, dateTime)")]
            public static readonly string start_ts = "start_ts";
            [Description("Quantit� di pioggia calcolata in 3 ore dai pluviometri in millimetri di accumulo. (millimetre, double)")]
            public static readonly string ThreeHoursRainfall = "ThreeHoursRainfall";
            [Description("Fascia oraria espressa come valore. Valori suggeriti \"diurna\", \"notturna\", dove se non diversamente specificato tramite \"Ora inizio\" e \"Ora fine\", si assume che: - diurna: dalle 07:00 alle 18:59 - notturna: dalle 19:00 alle 06:59 (dimensionless, string)")]
            public static readonly string TimeSlotDescription = "TimeSlotDescription";
            [Description("Numero totale UrbanDataset acceduti (dimensionless, integer)")]
            public static readonly string TotalAccessedUDCount = "TotalAccessedUDCount";
            [Description("Potenza attiva totale (kilowatt, double)")]
            public static readonly string TotalActivePower = "TotalActivePower";
            [Description("Potenza apparente totale (kilovoltampere, double)")]
            public static readonly string TotalApparentPower = "TotalApparentPower";
            [Description("Percentuale di copertura nuvolosa sul totale. (dimensionless, double)")]
            public static readonly string TotalCloudCoveragePercentage = "TotalCloudCoveragePercentage";
            [Description("Numero degli utenti gestiti (dimensionless, integer)")]
            public static readonly string TotalManagedUsersCount = "TotalManagedUsersCount";
            [Description("Numero delle applicazioni verticali gestite (dimensionless, integer)")]
            public static readonly string TotalManagedVerticalsCount = "TotalManagedVerticalsCount";
            [Description("Numero totale UrbanDataset prodotti (dimensionless, integer)")]
            public static readonly string TotalProducedUDCount = "TotalProducedUDCount";
            [Description("Potenza reattiva totale (kilovoltamperereactive, double)")]
            public static readonly string TotalReactivePower = "TotalReactivePower";
            [Description("codice ISTAT del comune (dimensionless, string)")]
            public static readonly string TownCode = "TownCode";
            [Description("Nome esteso del Comune es. Roma, Anguillara Sabazia, ... (dimensionless, string)")]
            public static readonly string TownName = "TownName";
            [Description("Codice identificativo dell'anomalia dell'impianto. (dimensionless, string)")]
            public static readonly string TreatmentPlantAnomalyCode = "TreatmentPlantAnomalyCode";
            [Description("dato catastale immobile : sezione urbana (dimensionless, string)")]
            public static readonly string UrbanSection = "UrbanSection";
            [Description("Nome della solution verticale (dimensionless, string)")]
            public static readonly string VerticalSolutionName = "VerticalSolutionName";
            [Description("Composti organici volatili (partsPerMillion, double)")]
            public static readonly string VOC = "VOC";
            [Description("Tensione della fase (volt, double)")]
            public static readonly string VoltagePhase = "VoltagePhase";
            [Description("Tensione della fase R (volt, double)")]
            public static readonly string VoltagePhase1 = "VoltagePhase1";
            [Description("Tensione della fase S (volt, double)")]
            public static readonly string VoltagePhase2 = "VoltagePhase2";
            [Description("Tensione della fase T (volt, double)")]
            public static readonly string VoltagePhase3 = "VoltagePhase3";
            [Description("Flusso acqua reflua specifica (cubicMetrePerSecond-Time, double)")]
            public static readonly string WasteWaterFlowRate = "WasteWaterFlowRate";
            [Description("Parametro che definisce la situazione meteo (pioggia, neve, sereno, coperto,�) (dimensionless, string)")]
            public static readonly string WeatherConditionCode = "WeatherConditionCode";
            [Description("Descrizione testuale della situazione meteo. (dimensionless, string)")]
            public static readonly string WeatherConditionDescription = "WeatherConditionDescription";
            [Description("luminosit� verticale direzione ovest (lux, double)")]
            public static readonly string WestLuminosity = "WestLuminosity";
            [Description("Direzione del vento espressa in gradi decimali rispetto al Nord geografico (misurata in senso orario) (degree, double)")]
            public static readonly string WindDirection = "WindDirection";
            [Description("Stato della finestra (aperto o chiuso) (dimensionless, string)")]
            public static readonly string WindowStatusCode = "WindowStatusCode";
            [Description("Velocit� del vento osservata in metri al secondo (metrePerSecond-Time, double)")]
            public static readonly string WindSpeed = "WindSpeed";
            [Description("Indica se si fa riferimento a giorni feriali o festivi. (dimensionless, string)")]
            public static readonly string WorkingDayFlag = "WorkingDayFlag";
            [Description("URL dell'endpoint del Web Service a cui si vuole accedere (dimensionless, string)")]
            public static readonly string WSEndpoint = "WSEndpoint";
            [Description("Codice di Avviamento Postale (CAP) eccezione nelle regole di naming in quanto non ha associato nessuna lista .gc (dimensionless, string)")]
            public static readonly string ZIPCode = "ZIPCode";
        }

        [DataMember(Name="propertyName")]
        public string PropertyName { get; set; }

        [DataMember(Name="propertyDescription")]
        public string PropertyDescription { get; set; }

        /// <value>
        /// Property <c>DataType</c> represents the type of data with which the property value is expressed. Examples:
        /// "dateTime", "double", "enumeration", "integer", "string".
        /// </value>
        [DataMember(Name="dataType", IsRequired = false)]
        public string DataType { get; set; }

        /// <value>
        /// Property <c>UnitOfMeasure</c> represents the unit of measurement in which the value is expressed.
        /// Examples: "ampere", "cubicMetrePerSecond-Time", "degree", "degreeCelsius", "dimensionless", "hectopascal",
        /// "kilovoltampere", "kilovoltamperereactive", "kilovoltamperereactivePerHour", "kilowatt", "kilowattHour",
        /// "lux", "metrePerSecond-Time", "microgramPerCubicCentimetre", "millimetre", "partsPerMillion",
        /// "squareMetre", "volt", "wattPerSquareMetre".
        /// </value>
        [DataMember(Name="unitOfMeasure", IsRequired = false)]
        public string UnitOfMeasure { get; set; }

        [DataMember(Name="subProperties", IsRequired = false)]
        public SubProperties SubProperties { get; set; }

        [DataMember(Name="codeList", IsRequired = false)]
        public Uri CodeList { get; set; }
    }

    [DataContract]
    public partial class SubProperties
    {
        [DataMember(Name="propertyName")]
        public List<string> PropertyName { get; set; }
    }

    [DataContract]
    public partial class Values
    {
        [DataMember(Name="line")]
        public List<Line> Line { get; set; }
    }

    [DataContract]
    public partial class Line
    {
        [DataMember(Name="id")]
        public long Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name="coordinates")]
        public Coordinates Coordinates { get; set; }

        [DataMember(Name="period")]
        public Period Period { get; set; }

        [DataMember(Name="property")]
        public List<Property> Property { get; set; }
    }

    [DataContract]
    public partial class Period
    {
        [DataMember(Name="start_ts")]
        public DateTime StartTs { get; set; }

        [DataMember(Name="end_ts")]
        public DateTime EndTs { get; set; }
    }

    [DataContract]
    public partial class Property
    {
        [DataMember(Name="name")]
        public string Name { get; set; }

        [DataMember(Name="val")]
        public string Val { get; set; }

        [DataMember(Name = "property")]
        public List<Property> SubProperties { get; set; }
    }
}