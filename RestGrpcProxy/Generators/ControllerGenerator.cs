using Microsoft.CodeAnalysis;
using RestGrpcProxy.Models;

namespace RestGrpcProxy.Generators
{
    public class ControllerGenerator
    {
        public static IEnumerable<string> Generate(IEnumerable<ServiceDefinition> serviceDefinitions)
        {
            var controllerTemplate = File.ReadAllText(Path.Combine("Templates","ControllerTemplate.txt"));
            var methodTemplate = File.ReadAllText(Path.Combine("Templates", "MethodTemplate.txt"));

            foreach (var serviceDefinition in serviceDefinitions)
            {
                var controllerSource = controllerTemplate.Replace("$CONTROLLER_NAME",
                    $"{serviceDefinition.ServiceName}Controller");

                var methodSrting = "";
                foreach (var endoint in serviceDefinition.EndpointDefinitions)
                {
                    var method = methodTemplate.Replace("$NAME", endoint.Name)
                        .Replace("$ROUTE", endoint.Name.ToLower());

                    var httpAttribute = "HttpPost";
                    //if (!endoint.Input.Properties.Any())
                    //    httpAttribute = "HttpGet";

                    method = method.Replace("$RESPONSE_TYPE", "RestGrpcProxy.Models." + endoint.Output.Name);
                    method = method.Replace("$INPUT_TYPE", "RestGrpcProxy.Models." + endoint.Input.Name);
                    methodSrting += method.Replace("$HTTP_ATTRIBUTE", httpAttribute);
                }

                controllerSource = controllerSource.Replace("$METHODS", methodSrting);
                
                yield return controllerSource;
            }
        }


    }
}
