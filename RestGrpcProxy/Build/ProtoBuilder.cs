using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml;

namespace RestGrpcProxy.Build
{
    public class ProtoBuilder
    {
        public static Assembly Build()
        {
            var protoDir = new DirectoryInfo("Protos");

            var outPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            outPath = Path.Combine(outPath, "RestGrpcProxy", "Generated", "GrpcLib.csproj");

            var output = new FileInfo(outPath);
            var protoFiles = protoDir.EnumerateFiles("*.proto", SearchOption.AllDirectories);

            PrepareOutputDir(output.Directory);

            var copyPath = Path.Combine(output.DirectoryName, "Protos");

            CopyProtoFiles(protoFiles, new DirectoryInfo(copyPath));
            CreateCsprojFile(protoFiles, output);
            
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
            {
                Console.WriteLine($"Deleteing output dir {outputDir.FullName}");
                outputDir.Delete(true);
            }

            Console.WriteLine($"Create output dir {outputDir.FullName}");
            outputDir.Create();
        }

        private static void BuildCsproj(FileInfo csProjFile)
        {
            Console.WriteLine("Start building csproj of grpc lib...");
            var proc = new Process();

            proc.StartInfo.WorkingDirectory = csProjFile.DirectoryName;
            proc.StartInfo.FileName = "dotnet";
            proc.StartInfo.Arguments = $"build --output={csProjFile.DirectoryName}/out " + csProjFile;

            proc.Start();

            proc.WaitForExit();

            proc.CloseMainWindow();
            proc.Close();
            Console.WriteLine("Done building grpc lib.");
        }

        private static void EnsureCsharpNamespace(FileInfo protoFile, string optionName)
        {
            var content = File.ReadAllText(protoFile.FullName);

            if (content.Contains("option csharp_namespace"))
                return;

            var matches = Regex.Match(content, $".*?option\\s+?{optionName}\\s+?=\\s+?\\\"(.*?)\\\";");

            if(matches.Groups.Count >= 2)
                content = content += $"\noption csharp_namespace = \"{matches.Groups[1].Value}\";";

            File.WriteAllText(protoFile.FullName, content);  
        }

        private static void CopyProtoFiles(IEnumerable<FileInfo> protoFiles, DirectoryInfo destination)
        {
            if (!destination.Exists)
                destination.Create();

            foreach (var protoFile in protoFiles)
            {
                var rel = Path.GetRelativePath("Proto", protoFile.FullName);
                var destinationPath = Path.Combine(destination.FullName, rel);

                Console.WriteLine($"Copy {protoFile.FullName} to {destinationPath}");

                if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

               File.Copy(protoFile.FullName, destinationPath);

               EnsureCsharpNamespace(new FileInfo(destinationPath), "java_package");
            }
        }

        private static void CreateCsprojFile(IEnumerable<FileInfo> protoFiles, FileInfo destination)
        {
            //< ItemGroup >
            //  < Protobuf Include = "Protos\acars\acarsTemplate.proto" GrpcServices = "Client" />
            //</ ItemGroup >

            Console.WriteLine($"Start creating csproj file for grpc lib at {destination.FullName}");
            var grpcLibProjectFile = new XmlDocument();

            grpcLibProjectFile.Load("Templates/CsprojTemplate.xml");

            var projectNode = grpcLibProjectFile.DocumentElement;

            var itemGroup = grpcLibProjectFile.CreateElement("ItemGroup");

            foreach (var protoFile in protoFiles)
            {
                var protobufNode = grpcLibProjectFile.CreateElement("Protobuf");
                protobufNode.SetAttribute("GrpcServices", "Client");

                var rel = Path.GetRelativePath("Protos", protoFile.FullName);
                protobufNode.SetAttribute("Include", @$"Protos\{rel}");

                protobufNode.SetAttribute("ProtoRoot", "Protos");

                itemGroup.AppendChild(protobufNode);
            }

            projectNode.AppendChild(itemGroup);

            grpcLibProjectFile.Save(destination.FullName);
            Console.WriteLine("Done creating csproj file.");
        }
    }
}