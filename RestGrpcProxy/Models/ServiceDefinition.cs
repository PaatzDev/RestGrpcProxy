namespace RestGrpcProxy.Models
{
    public class ServiceDefinition
    {
        public string Namespace { get; set; }
        public string ServiceName { get; set; }
        public List<EndpointDefinition> EndpointDefinitions { get; set; } = new List<EndpointDefinition>();
        public static List<MessageDefinition> MessageDefinitions { get; set; } = new List<MessageDefinition>();
        public static List<Type> CompiledMessages { get; set; }
    }
}
