using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using TriggerAction.ServiceModel;
using TriggerAction.ServiceModel.Enums;
using TriggerAction.ServiceModel.Types;

namespace TriggerAction.ServiceInterface
{
    public class DeviceService : Service
    {
        public object Any(DeviceRequest request)
        {
            var dev = Db.LoadSingleById<Device>(request.DeviceId);
            if (dev == null) return null;

            var dto = dev.ConvertTo<DeviceResponse>();

            Db.LoadReferences(dev.DeviceType);
            // dto.SensorTypeCode = dev.DeviceType.DeviceCategory.Label;
            //dto.SensorTypeCode = SensorTypeCode.ConfortMultisensor;


            // TODO: SensorName, SensorTypeCode.
            // Per SensorTypeCode si veda http://smartcityplatform.enea.it/specification/semantic/1.0/gc/SensorTypeCode.gc

            var sql =
                "SELECT [Id] = B.[DataValueId]" +
                ", A.[DeviceId]" +
                ", B.[Timestamp]" +
                ", [Label] = ISNULL(C.[Label], A.[Label])" +
                ", [Value] = (B.[Value] + ISNULL(C.[Offset], 0)) * ISNULL(C.[TransformFactor], 1)" +
                ", [Unit] = ISNULL(C.[Unit], A.[Unit])" +
                ", C.[BatchOperationType]" +
                " FROM (" +
                "SELECT [Devices].[DeviceId]" +
                ", [DeviceTypeValues].[ValueTypeId]" +
                ", [ValueTypes].[Label]" +
                ", [ValueTypes].[Unit]" +
                " FROM [ValueTypes]" +
                " JOIN [DeviceTypeValues] ON [DeviceTypeValues].[ValueTypeId] = [ValueTypes].[ValueTypeId]" +
                " JOIN [Devices] ON [Devices].[DeviceTypeId] = [DeviceTypeValues].[DeviceTypeId]" +
                " LEFT JOIN [DeviceValues] ON [DeviceValues].[DeviceId] = [Devices].[DeviceId]" +
                " AND [DeviceValues].[ValueTypeId] = [DeviceTypeValues].[ValueTypeId] " +
                " WHERE [Devices].[DeviceId] = @DeviceId" +
                ") A" +
                " CROSS APPLY (" +
                "SELECT TOP 1" +
                " [DataValues].[DataValueId]" +
                ", [DataValues].[Value]" +
                ", [DataValues].[Error]" +
                ", [DataPackets].[Timestamp]" +
                " FROM [DataPackets]" +
                " JOIN [DataValues] ON [DataValues].[PacketId] = [DataPackets].[PacketId]" +
                " WHERE [DataPackets].[DeviceId] = A.[DeviceId]" +
                " AND [DataValues].[ValueTypeId] = A.[ValueTypeId]" +
                " ORDER BY [DataPackets].[Timestamp] DESC" +
                ") B" +
                " JOIN (" +
                "SELECT [DeviceId]" +
                ", [ValueTypeId]" +
                ", [Offset]" +
                ", [Unit]" +
                ", [TransformFactor]" +
                ", [Label]" +
                ", [BatchOperationType]" +
                " FROM [DeviceValues]" +
                " WHERE [BatchOperationType] LIKE 'SCP-%'" +
                ") C ON A.[ValueTypeId] = C.[ValueTypeId] AND A.[DeviceId] = C.[DeviceId]";

            // Al momento dobbiamo supporre che il fuso orario sia lo stesso della macchina su cui gira il servizio,
            // perciò eliminiamo i record con marca temporale non valida od ambigua. Successivamente riporteremo tutto
            // al fuso orario corrente.

            var lastReadings = Db.Select<Reading>(sql, new { request.DeviceId })
                .SafeWhere(x => x.Timestamp != null && !TimeZoneInfo.Local.IsInvalidTime(x.Timestamp.DateTime) && !TimeZoneInfo.Local.IsAmbiguousTime(x.Timestamp.DateTime))
                .ToList();

            if (lastReadings.Count > 0)
            {
                foreach (var reading in lastReadings)
                {
                    // Riportiamo tutte le letture al fuso orario corrente.
                    var dateTime = reading.Timestamp.DateTime;
                    var offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
                    reading.Timestamp = new DateTimeOffset(dateTime, offset);

                    // Verifichiamo il formato dei decimali. TODO: Meglio sarebbe personalizzare il "converter".
                    // Vedere: https://github.com/ServiceStack/ServiceStack.OrmLite/blob/master/src/ServiceStack.OrmLite/Converters/StringConverter.cs#L61

                    if (double.TryParse(reading.Value, out double result))
                    {
                        reading.Value = result.ToString(CultureInfo.InvariantCulture);
                    }
                }

                // Al momento dobbiamo supporre che le grandezze siano tutte istantanee, perciò impostiamo un "Period"
                // in cui "EndTs" coincide con "StartTs".

                var start_ts = lastReadings.Min(x => x.Timestamp);
                dto.Period = new ServiceModel.Period { StartTs = start_ts, EndTs = start_ts };
            }

            /*
             * Fanno parte delle "letture" anche alcune informazioni sui dispositivi, per esempio il "SensorID".
             * Quest'ultimo assume la sintassi [Label] + "-" [Id] dove [Label] è una stringa costituita da lettere
             * maiuscole e numeri, ed [Id] è un identificativo numerico. Se la [SensorLabel] nella base dati ha già
             * un formato simile la prendiamo testualmente, altrimenti costruiamo il "SensorID" utilizzando come [Id]
             * la chiave primaria.
             */

            var BatchOperationType = "SCP-"; // Uguale per tutte le informazioni aggiuntive.
            var DeviceId = request.DeviceId;

            var SensorID = Regex.IsMatch(dto.SensorLabel.Trim(), @"^[a-zA-Z0-9]+-[0-9]+$")
                ? dto.SensorLabel.Trim().ToUpperInvariant()
                : Regex.Replace(dto.SensorLabel, @"[^a-zA-Z0-9]", string.Empty).ToUpperInvariant() + "-" + dto.Id;

            lastReadings.Add(new Reading
            {
                BatchOperationType = BatchOperationType,
                DeviceId = DeviceId,
                Label = PropertyDefinition.CommonPropertyNames.SensorID,
                Unit = "dimensionless",
                Value = SensorID,
                Timestamp = DateTimeOffset.Now
            });

            string realEstateUnitID;
            string roomCode;

            if (Regex.IsMatch(dev.Location, @"(?:[A-Za-z0-9]+)(?:\s*,\s*(?:[A-Za-z0-9]+))+"))
            {
                var strList = dev.Location.Split(',')
                    .Select(x => x.Trim().ToUpperInvariant())
                    .ToList();

                realEstateUnitID = strList[0];
                roomCode = int.TryParse(strList[1], out int value) && Enum.IsDefined(typeof(RoomCode), value)
                    ? value.ToString()
                    : RoomCode.Altro.ToString("d");
            }
            else
            {
                realEstateUnitID = Regex.Replace(dev.Plant.Label.Trim(), @"[^a-zA-Z0-9]", string.Empty).ToUpperInvariant() + "-" + dev.PlantId;
                roomCode = RoomCode.Altro.ToString("d");
            }

            lastReadings.Add(new Reading
            {
                BatchOperationType = BatchOperationType,
                DeviceId = DeviceId,
                Label = PropertyDefinition.CommonPropertyNames.RealEstateUnitID,
                Unit = "dimensionless",
                Value = realEstateUnitID,
                Timestamp = DateTimeOffset.Now
            });

            lastReadings.Add(new Reading
            {
                BatchOperationType = BatchOperationType,
                DeviceId = DeviceId,
                Label = PropertyDefinition.CommonPropertyNames.RoomCode,
                Unit = "dimensionless",
                Value = roomCode,
                Timestamp = DateTimeOffset.Now
            });

            dto.Values = lastReadings;
            return dto;
        }

        public object Get(PlantsRequest req)
        {
            return Db.LoadSelect<Plant>();
        }
    }
}