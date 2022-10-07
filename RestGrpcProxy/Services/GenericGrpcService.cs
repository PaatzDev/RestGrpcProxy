using Grpc.Core;
using Grpc.Net.Client;

namespace RestGrpcProxy.Services
{
    public class GenericGrpcService<TService> where TService : class
    {
        public GenericGrpcService(string address)
        {
            var channel = GrpcChannel.ForAddress(address);

            GrpcClient = Activator.CreateInstance(typeof(TService), channel) as TService;
        }

        public TService? GrpcClient { get; private set; }
    }
}
