using System.ComponentModel;

namespace TriggerAction.ServiceModel.Codes
{
    public static class RoomCode
    {
        [Description("Cucinotto")]
        public static string Kitchenette = "KTH-R";
        [Description("Cucina")]
        public static string Kitchen = "KTC-R";
        [Description("Soggiorno")]
        public static string LivingRoom = "LVN-R";
        [Description("Sala/Salotto")]
        public static string DrawingRoom = "DRW-R";
        [Description("Bagno")]
        public static string Bathroom = "BTH-R";
        [Description("Camera")]
        public static string Bedroom = "BED-R";
        [Description("Studio")]
        public static string Study = "STD-R";
        [Description("Corridoio")]
        public static string Corridor = "CRR-R";
        [Description("Lavanderia")]
        public static string LaundryRoom = "LND-R";
        [Description("Cantina")]
        public static string Cellar = "CLL-R";
        [Description("Ufficio")]
        public static string Office = "OFF-R";
        [Description("Aula")]
        public static string Classroom = "CLS-R";
        [Description("Aula magna")]
        public static string Auditorium = "ADT-R";
        [Description("Biblioteca")]
        public static string Library = "LBR-R";
        [Description("Stanza Degenza")]
        public static string PatientRoom = "PTN-R";
        [Description("Zona Giorno")]
        public static string LivingArea = "LVN-A";
        [Description("Zona Notte")]
        public static string SleepingArea = "SLP-A";
        [Description("Zona Servizi")]
        public static string ServiceArea = "SRV-A";
        [Description("Spazio Comune")]
        public static string CommonArea = "CMM-A";
        [Description("Altro")]
        public static string Other = "OTH-R";

        public static bool IsValid(string code)
        {
            return code == Kitchenette
                || code == Kitchen
                || code == LivingRoom
                || code == DrawingRoom
                || code == Bathroom
                || code == Bedroom
                || code == Study
                || code == Corridor
                || code == LaundryRoom
                || code == Cellar
                || code == Office
                || code == Classroom
                || code == Auditorium
                || code == Library
                || code == PatientRoom
                || code == LivingArea
                || code == SleepingArea
                || code == ServiceArea
                || code == CommonArea
                || code == Other;
        }
    }
}
