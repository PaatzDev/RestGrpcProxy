namespace RestGrpcProxy.Models
{
    public class MessageDefinition
    {
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
