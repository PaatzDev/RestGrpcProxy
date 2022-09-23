using Microsoft.CodeAnalysis;
using RestGrpcProxy.Models;

namespace RestGrpcProxy
{
    public class ControllerGenerator
    {
        public static IEnumerable<string> Generate(IEnumerable<ServiceDefinition> serviceDefinitions)
        {
            foreach(var serviceDefinition in serviceDefinitions)
            {
                var controllerTemplate = File.ReadAllText("ControllerTemplate.txt");
                var methodTemplate = File.ReadAllText("MethodTemplate.txt");

                controllerTemplate = controllerTemplate.Replace("$CONTROLLER_NAME", 
                    $"{serviceDefinition.ServiceName}Controller");

                var methodSrting = "";
                foreach(var endoint in serviceDefinition.EndpointDefinitions)
                {
                    var method = methodTemplate.Replace("$NAME", endoint.Name)
                        .Replace("$ROUTE", endoint.Name);

                    methodSrting += method;
                }

                controllerTemplate = controllerTemplate.Replace("$METHODS", methodSrting);

                File.WriteAllText("test.cs", controllerTemplate);

                yield return controllerTemplate;
            }
            
        }

       
    }
}
