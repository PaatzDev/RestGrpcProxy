using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using RestGrpcProxy.Build;
using System.Reflection;

namespace RestGrpcProxy
{
    public class ControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var compiledAssembly = Compiler.Compile();

            var assembly = Assembly.Load(compiledAssembly);

            var controllerTypes = assembly.GetTypes().Where(x => x.GetCustomAttributes(typeof(ApiControllerAttribute)).Any());

            foreach (var controller in controllerTypes)
                    feature.Controllers.Add(controller.GetTypeInfo());
        }
    }
}
