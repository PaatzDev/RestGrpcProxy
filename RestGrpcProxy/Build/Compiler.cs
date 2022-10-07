using Grpc.Core;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using RestGrpcProxy.Generators;
using RestGrpcProxy.Models;
using RestGrpcProxy.Parser;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace RestGrpcProxy.Build
{
    public class Compiler
    {
        public static byte[] CompileGrpcAssambly(DirectoryInfo location)
        {
            var syntaxTrees = new List<SyntaxTree>();

            foreach(var file in location.EnumerateFiles())
            {
                var source = File.ReadAllText(file.FullName);
                var syntaxTree = BuildSyntaxTree(source);

                if (syntaxTree != null)
                    syntaxTrees.Add(syntaxTree);
            }

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var references = new MetadataReference[] {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Generic.IEnumerable<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpPostAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpGetAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.IActionResult).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(typeof(Google.Protobuf.IMessage).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerResponseAttribute).Assembly.Location)
            };

            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                false, null, null, null, null,
                OptimizationLevel.Release,
                false, false, null, null, default, false,
                Platform.AnyCpu, ReportDiagnostic.Default
                , 4, null, true, false, null, null, null,
                AssemblyIdentityComparer.Default);

            var compilation = CSharpCompilation.Create("DynamicGrpcAssembly.dll",
                syntaxTrees,
                references,
                compilationOptions);

            using (var peStream = new MemoryStream())
            {
                var result = compilation.Emit(peStream);

                if (!result.Success)
                    return null;

                peStream.Seek(0, SeekOrigin.Begin);

                return peStream.ToArray();
            }
        }

        public static byte[] CompileRestAssembly(ref IEnumerable<ServiceDefinition> serviceDefinitions, Assembly grpcLib)
        {
            var syntaxTrees = new List<SyntaxTree>();

            CreateMessageObjectsSyntaxTrees(ServiceDefinition.MessageDefinitions, ref syntaxTrees);
            CreateControllerSyntaxTrees(serviceDefinitions, ref syntaxTrees);

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var references = new MetadataReference[] {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.Generic.IEnumerable<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpPostAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpGetAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.IActionResult).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RestGrpcProxy.Services.GenericGrpcService<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Grpc.Core.ClientBase).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ValueType).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Swashbuckle.AspNetCore.Annotations.SwaggerResponseAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(grpcLib.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "netstandard.dll")),
            };

            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                false, null, null, null, null,
                OptimizationLevel.Release,
                false, false, null, null, default, false,
                Platform.AnyCpu, ReportDiagnostic.Default
                , 4, null, true, false, null, null, null,
                AssemblyIdentityComparer.Default);

            var compilation = CSharpCompilation.Create("DynamicAssembly.dll",
                syntaxTrees,
                references,
                compilationOptions);

            using(var peStream = new MemoryStream())
            {
                var result = compilation.Emit(peStream);

                if (!result.Success)
                    return null;

                peStream.Seek(0, SeekOrigin.Begin);

                return peStream.ToArray();
            }
        }

        private static void CreateMessageObjectsSyntaxTrees(List<MessageDefinition> messageDefinitions, ref List<SyntaxTree> syntaxTrees)
        {
           var messagesSourceCodes = ObjectGenerator.Generate(messageDefinitions);
            foreach(var sourceCode in messagesSourceCodes)
            {
                var syntaxTree = BuildSyntaxTree(sourceCode);

                if (syntaxTree != null)
                    syntaxTrees.Add(syntaxTree);
            }
        }

        private static void CreateControllerSyntaxTrees(IEnumerable<ServiceDefinition> serviceDefinitions, ref List<SyntaxTree> syntaxTrees)
        {
            var controllerSources = ControllerGenerator.Generate(serviceDefinitions);
            foreach(var controllerSource in controllerSources)
            {
                var syntaxTree = BuildSyntaxTree(controllerSource);

                if (syntaxTree != null)
                    syntaxTrees.Add(syntaxTree);
            }
        }

        private static SyntaxTree BuildSyntaxTree(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.LatestMajor);

            var parsedSyntaxTree = CSharpSyntaxTree.ParseText(codeString, options);
            return parsedSyntaxTree;
        }

        public static byte[] Compile(Assembly grpcLib)
        {
            var serviceDefinitions = ProtoParser.Parse("Protos");

            var compiledAssembly = CompileRestAssembly(ref serviceDefinitions, grpcLib);

            return compiledAssembly;
        }   
    }
}
