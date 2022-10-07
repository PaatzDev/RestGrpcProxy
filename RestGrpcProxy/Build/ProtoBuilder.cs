using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;

namespace RestGrpcProxy.Build
{
    public class ProtoBuilder
    {
        public static Assembly Build()
        {
            var protoDir = new DirectoryInfo("Protos");
            var output = new FileInfo(@"F:\Projects\Test\GrpcLib.csproj");

            PrepareOutputDir(output.Directory);

            CreateCsprojFile(protoDir.EnumerateFiles("*.proto", SearchOption.TopDirectoryOnly), output);

            BuildCsproj(output);

            return LoadAssembly(output.Directory);
        }

        private static Assembly LoadAssembly(DirectoryInfo dllOutDir)
        {
            var dllFile = Path.Combine(dllOutDir.FullName, "out", "GrpcLib.dll");

            if (!File.Exists(dllFile))
                return null;

            return AssemblyBuilder.LoadFrom(dllFile);
        }


        private static void PrepareOutputDir(DirectoryInfo outputDir)
        {
            if (outputDir.Exists)
                outputDir.Delete(true);

            outputDir.Create();
        }

        private static void BuildCsproj(FileInfo csProjFile)
        {
            var proc = new Process();

            proc.StartInfo.WorkingDirectory = csProjFile.DirectoryName;
            proc.StartInfo.FileName = "dotnet";
            proc.StartInfo.Arguments = $"build --output={csProjFile.DirectoryName}/out " + csProjFile;

            proc.Start();

            proc.WaitForExit();

            proc.CloseMainWindow();
            proc.Close();
        }

        private static void CreateCsprojFile(IEnumerable<FileInfo> protoFiles, FileInfo destination)
        {
            var grpcLibProjectFile = new XmlDocument();

            grpcLibProjectFile.Load("Templates/CsprojTemplate.xml");

            var projectNode = grpcLibProjectFile.DocumentElement;

            var itemGroup = grpcLibProjectFile.CreateElement("ItemGroup");

            foreach (var protoFile in protoFiles)
            {
                var protobufNode = grpcLibProjectFile.CreateElement("Protobuf");
                protobufNode.SetAttribute("GrpcServices", "Client");
                protobufNode.SetAttribute("Include", protoFile.FullName);

                var linkNode = grpcLibProjectFile.CreateElement("Link");
                linkNode.InnerText = @$"Protos\{protoFile.Name}";

                protobufNode.AppendChild(linkNode);

                itemGroup.AppendChild(protobufNode);
            }

            projectNode.AppendChild(itemGroup);

            
            grpcLibProjectFile.Save(destination.FullName);
        }
    }
}