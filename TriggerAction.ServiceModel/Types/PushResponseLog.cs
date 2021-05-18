using ServiceStack;
using ServiceStack.Model;

namespace TriggerAction.ServiceModel.Types
{
    [NamedConnection("Local")]
    public class PushResponseLog : IHasId<int>
    {
        public int Id { get; set; }
        public long UnixTimeMilliseconds { get; set; }
        public PushResponse PushResponse { get; set; }
    }

    public class QueryPushResponseLog : QueryDb<PushResponseLog> { }
}
