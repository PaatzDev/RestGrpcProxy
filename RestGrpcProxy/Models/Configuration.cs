namespace RestGrpcProxy.Models
{
    public class Configuration
    {
        public string ServiceName { get; set;}
        public string ServiceDescription { get; set;}
        public string ServiceVersion { get; set;}

        public IEnumerable<AddressMap> addresses;
    }
}
