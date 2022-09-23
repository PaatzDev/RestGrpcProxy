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
            var asm = Compiler.Compile();
            var controllers = Compiler.Execute(asm);

            foreach (var controller in controllers)
                feature.Controllers.Add(controller.GetTypeInfo());
        }
    }
}
