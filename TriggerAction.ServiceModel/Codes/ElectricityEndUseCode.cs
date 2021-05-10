using System.ComponentModel;

namespace TriggerAction.ServiceModel.Codes
{
    public static class ElectricityEndUseCode
    {
        [Description("Illuminazione")]
        public static string Lighting = "LGH";
        [Description("Condizionamento")]
        public static string AirConditioning = "CND";
        [Description("Forza Motrice")]
        public static string Plug = "PWR";
        [Description("Generale")]
        public static string Master = "ALL";

        public static bool IsValid(string code)
        {
            return code == Lighting
                || code == AirConditioning
                || code == Plug
                || code == Master;
        }
    }
}
