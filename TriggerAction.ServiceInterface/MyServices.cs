using ServiceStack;
using TriggerAction.ServiceModel;

namespace TriggerAction.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }

        public object Get(TestRequest request)
        {
            return new TestResponse() { Code = "00", Message = "Succesful" };
        }
    }
}
