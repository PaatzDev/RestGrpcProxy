using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using RestGrpcProxy.Generators;
using RestGrpcProxy.Models;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace RestGrpcProxy.Build
{
    public class Compiler
    {
        public static byte[] Compile()
        {
            var serviceDefinitions = Protos.ProtoParser.Parse("Protos");

            var messagesSource = ObjectGenerator.Generate(ServiceDefinition.MessageDefinitions);

            var source = ControllerGenerator.Generate(serviceDefinitions);

            using (var peStream = new MemoryStream())
            {
                var result = GenerateCode(source.First()).Emit(peStream);

                if (!result.Success)
                {
                    Console.WriteLine("Compilation done with error.");

                    return null;
                }

                Console.WriteLine("Compilation done without any error.");

                peStream.Seek(0, SeekOrigin.Begin);

                return peStream.ToArray();
            }
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.LatestMajor);

            var parsedSyntaxTree = CSharpSyntaxTree.ParseText(codeString, options);

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            var references = new MetadataReference[] {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpPostAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.HttpGetAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.RouteAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.IActionResult).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
            };

            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                false, null, null, null, null,
                OptimizationLevel.Release,
                false, false, null, null, default, false,
                Platform.AnyCpu, ReportDiagnostic.Default
                , 4, null, true, false, null, null, null,
                AssemblyIdentityComparer.Default);


            return CSharpCompilation.Create("test.dll",
                new[] { parsedSyntaxTree },
                references,
                compilationOptions);
        }

        public static IEnumerable<Type> Execute(byte[] compiledAssembly)
        {
            return LoadAndExecute(compiledAssembly);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static IEnumerable<Type> LoadAndExecute(byte[] compiledAssembly)
        {
            using (var asm = new MemoryStream(compiledAssembly))
            {
                var assemblyLoadContext = new SimpleUnloadableAssemblyLoadContext();

                var assembly = assemblyLoadContext.LoadFromStream(asm);

                var types = assembly.GetTypes();


                var currentAssembly = typeof(IApplicationFeatureProvider).Assembly;

                return types;
            }
        }
    }

    internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext()
            : base(true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}
