using System;

namespace TriggerAction.ServiceModel.Types
{
    public class Reading
    {
        private string value;

        public long? Id { get; set; }
        public int DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Label { get; set; }
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
            }
        }
        public string Unit { get; set; }
        public string BatchOperationType { get; set; }
    }
}
