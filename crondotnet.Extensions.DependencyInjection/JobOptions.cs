using crondotnet;

namespace Microsoft.Extensions.DependencyInjection
{
    public class JobOptions
    {
        public string Expression { get; set; }
        public ExecuteCronJob StaticTask { get; set; }
        public Type ServiceType { get; internal set; }
    }
}