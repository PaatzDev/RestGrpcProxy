namespace RestGrpcProxy.Models
{
    public class ServiceDefinition
    {
        public string PackageName { get; set; }
        public string ServiceName { get; set; }
        public List<EndpointDefinition> EndpointDefinitions { get; set; } = new List<EndpointDefinition>();
    }
}
