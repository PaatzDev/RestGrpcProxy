using Microsoft.CodeAnalysis;
using RestGrpcProxy.Models;

namespace RestGrpcProxy.Generators
{
    public class ControllerGenerator
    {
        public static IEnumerable<string> Generate(IEnumerable<ServiceDefinition> serviceDefinitions)
        {
            var controllerTemplate = File.ReadAllText("ControllerTemplate.txt");
            var methodTemplate = File.ReadAllText("MethodTemplate.txt");

            foreach (var serviceDefinition in serviceDefinitions)
            {
                var controllerSource = controllerTemplate.Replace("$CONTROLLER_NAME",
                    $"{serviceDefinition.ServiceName}Controller");

                var methodSrting = "";
                foreach (var endoint in serviceDefinition.EndpointDefinitions)
                {
                    var method = methodTemplate.Replace("$NAME", endoint.Name)
                        .Replace("$ROUTE", endoint.Name);

                    var httpAttribute = "HttpPost";
                    if (!endoint.Input.Properties.Any())
                        httpAttribute = "HttpGet";

                    methodSrting += method.Replace("$HTTP_ATTRIBUTE", httpAttribute);
                }

                controllerTemplate = controllerSource.Replace("$METHODS", methodSrting);

                yield return controllerTemplate;
            }

        }


    }
}
