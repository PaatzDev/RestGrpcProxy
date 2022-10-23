using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RestGrpcProxy.Models;
using RestGrpcProxy.Services;
using System.Text.RegularExpressions;

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

                controllerSource = controllerSource.Replace("$GRPC_SERVICE_TYPE", $"{serviceDefinition.Namespace}." +
                    $"{serviceDefinition.ServiceName}.{serviceDefinition.ServiceName}Client");

                var configService = new ConfigurationService();

                var address = configService.Config.addresses.Where(x => x.ServiceName == serviceDefinition.ServiceName)
                    .FirstOrDefault();

                if(address != null)
                    controllerSource = controllerSource.Replace("$ADDRESS", address.Address);

                var methodSrting = "";
                foreach (var endoint in serviceDefinition.EndpointDefinitions)
                {
                    var method = methodTemplate.Replace("$NAME", endoint.Name)
                        .Replace("$ROUTE", endoint.Name.ToLower());

                    method = method.Replace("$RESPONSE_TYPE", $"{endoint.Output.Namespace}.{endoint.Output.Name}");
                    method = method.Replace("$INPUT_TYPE", $"{endoint.Input.Namespace}.{endoint.Input.Name}");

                    var httpAttribute = "HttpPost";
                    if (!endoint.Input.Properties.Any())
                    {
                        httpAttribute = "HttpGet";
                        method = Regex.Replace(method, @"\[FromBody].+? input", "");
                        method = method.Replace("input", $"new {endoint.Input.Namespace}.{endoint.Input.Name}()");
                    }

                    methodSrting += method.Replace("$HTTP_ATTRIBUTE", httpAttribute);
                }

                controllerSource = controllerSource.Replace("$METHODS", methodSrting);
                
                yield return controllerSource;
            }
        }
    }
}
