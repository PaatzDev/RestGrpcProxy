using Grpc.Core;
using RestGrpcProxy.Protos;

namespace GrpcService.Services
{
    public class TestGrpcService : RestGrpcProxy.Protos.TestService.TestServiceBase
    {
        private readonly ILogger<TestGrpcService> _logger;

        public TestGrpcService(ILogger<TestGrpcService> logger)
        {
            _logger = logger;   
        }

        public override async Task<TestValue> GetTestValue(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("New request from: " + context.Peer);

            var testValue = new TestValue();

            testValue.Amount = 100;
            testValue.Id = "testvalue";

            return testValue;
        }

        public override async Task<TestValues> GetTestValues(PageInfo request, ServerCallContext context)
        {
            _logger.LogInformation("New request from: " + context.Peer);

            var testValues = new TestValues();

            testValues.Values.AddRange(new[]
            {
                new TestValue{ Id = Guid.NewGuid().ToString(), Amount = 1 }, 
                new TestValue{ Id = Guid.NewGuid().ToString(), Amount = 45 }, 
                new TestValue{ Id = Guid.NewGuid().ToString(), Amount = 256 }, 
            });

            return testValues;
        }
    }
}
