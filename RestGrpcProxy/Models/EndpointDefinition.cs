namespace RestGrpcProxy.Models
{
    public class EndpointDefinition
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public MessageDefinition Input { get; set; }
        public MessageDefinition Output { get; set; }
    }
}
