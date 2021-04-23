using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerAction.ServiceModel.Types
{
    [NamedConnection("Reporting")]
    public class PushResponseLog : IHasId<int>
    {
        public int Id { get; set; }
        public long UnixTimeMilliseconds { get; set; }
        public PushResponse PushResponse { get; set; }
    }

    public class QueryPushResponseLog : QueryDb<PushResponseLog> { }
}
