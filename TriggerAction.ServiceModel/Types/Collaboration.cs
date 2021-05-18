using ServiceStack;
using ServiceStack.Model;

namespace TriggerAction.ServiceModel.Types
{
    [NamedConnection("Local")]
    public class Collaboration : IHasId<int>,
        IUpdateDb<Collaboration>, IReturn<UpdateCollaborationResponse>
    {
        public int Id { get; set; }
        public string ResourceId { get; set; }
        public string Username { get; set; }
    }

    public class UpdateCollaborationResponse
    {
        public int Id { get; set; }
        public Collaboration Result { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class QueryCollaboration : QueryDb<Collaboration> { }

    public class CreateCollaboration :
        ICreateDb<Collaboration>, IReturn<CreateCollaborationResponse>
    {
        public string ResourceId { get; set; }
        public string Username { get; set; }
    }

    public class CreateCollaborationResponse
    {
        public int Id { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
