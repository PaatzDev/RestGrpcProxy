using RestGrpcProxy.Models;

namespace RestGrpcProxy.Protos
{
    public class ProtoParser
    {

        public static IEnumerable<ServiceDefinition> Parse(string protoPath)
        {
            var dir = new DirectoryInfo(protoPath);

            var files = dir.GetFiles("*.proto", SearchOption.AllDirectories);

            foreach(var file in files)
            {
                yield return ProcessFile(file);
            }
        }

        private static ServiceDefinition ProcessFile(FileInfo file)
        {
            using (var fileStream = file.OpenRead()) {
                using (var reader = new StreamReader(fileStream))
                {
                    var packageName = "";
                    var serviceDefinition = new ServiceDefinition();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line.StartsWith("package"))
                        {
                            packageName = line.Replace("package", "").Replace(";", "").Trim();
                        }
                        else if (line.StartsWith("message"))
                        {
                            var messageName = line.Replace("message", "").Replace("{", "").Trim();
                        }
                        else if (line.StartsWith("service"))
                        {
                            var serviceName = line.Replace("service", "").Replace("{", "").Trim();

                            serviceDefinition.ServiceName = serviceName;
                            serviceDefinition.PackageName = packageName;
                        }
                        else if (line.Trim().StartsWith("rpc"))
                        {
                            var methodName = line.Replace("rpc ", "");

                            methodName = methodName.Substring(0, methodName.IndexOf("(")).Trim();


                            serviceDefinition.EndpointDefinitions.Add(new EndpointDefinition
                            {
                                Name = methodName,
                                Path = $"{serviceDefinition.ServiceName}/{methodName}".ToLower()
                            });
                        }
                    }

                    return serviceDefinition;
                }
            }
        }
    }
}
