﻿using RestGrpcProxy.Models;
using System.Text.RegularExpressions;

namespace RestGrpcProxy.Protos
{
    public class ProtoParser
    {
        private readonly static string _packagePattern = @"^(package)(\s+)([\w*.?]+)(;)";
        private readonly static string _servicePattern = @"^(service)(\s+)(\w+)(\s*)({?)";
        private readonly static string _rpcPattern = @"(rpc\s+)(\w+)(\s*\(\s*)(\w+)(\s*\)\s*returns\s*\(\s*)(\w+)(\s*\);)";

        public static IEnumerable<ServiceDefinition> Parse(string protoPath)
        {
            var dir = new DirectoryInfo(protoPath);

            var files = dir.GetFiles("*.proto", SearchOption.AllDirectories);

            var services = new List<ServiceDefinition>();
            var messages = new List<MessageDefinition>();

            GetMessageDefinitions(files, ref messages);
            ServiceDefinition.MessageDefinitions = messages;

            GetServiceDefinitions(files, ref services, ref messages);

            return services;
        }

        private static void GetMessageDefinitions(IEnumerable<FileInfo> files, ref List<MessageDefinition> messages)
        {
            foreach (var file in files)
            {
                using (var fileStream = file.OpenRead())
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine().Trim();

                            if (line.StartsWith("message"))
                            {
                                var matches = Regex.Match(line, @"^message\s+?(\w+)(?:\s*)");

                                var messageName = matches.Groups[1].Value;
                                var messageDefinition = new MessageDefinition { Name = messageName};

                                while (!line.Contains("}"))
                                {
                                    line = reader.ReadLine().Trim();

                                    matches = Regex.Match(line, @"^(\s*)([\d\w]+)\s([\d\w]+)");

                                    if (matches.Groups.Count > 2)
                                    {
                                        var propertyType = matches.Groups[2].Value;
                                        var propertyName = matches.Groups[3].Value;

                                        if (!messageDefinition.Properties.ContainsKey(propertyName))
                                        {
                                            messageDefinition.Properties.Add(propertyName, propertyType);
                                        }
                                    }
                                }

                                messages.Add(messageDefinition);
                            }
                        }
                    }
                }
            }
        }

        private static void GetServiceDefinitions(IEnumerable<FileInfo> files, ref List<ServiceDefinition> services,
            ref List<MessageDefinition> messages)
        {
            foreach (var file in files)
            {

                using (var fileStream = file.OpenRead())
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        var serviceDefinition = new ServiceDefinition();

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();

                            if (line.StartsWith("package"))
                            {
                                var matches = Regex.Match(line, _packagePattern);

                                if(matches.Groups.Count > 2)
                                {
                                    serviceDefinition.PackageName = matches.Groups[3].Value;
                                }                                
                            }
                            else if (line.StartsWith("service"))
                            {
                                var matches = Regex.Match(line, _servicePattern);

                                if(matches.Groups.Count > 2)
                                {
                                    serviceDefinition.ServiceName = matches.Groups[3].Value;
                                }
                            }
                            else if (line.Trim().StartsWith("rpc"))
                            {
                                var methodName = line.Replace("rpc ", "");

                                methodName = methodName.Substring(0, methodName.IndexOf("(")).Trim();

                                var matches = Regex.Match(line, _rpcPattern);

                                var inputProperty = new MessageDefinition { Name = "" };
                                var outputProperty = new MessageDefinition { Name = "" };

                                if (matches.Groups.Count > 2)
                                {
                                    if (messages.Any(x => x.Name == matches.Groups[4].Value))
                                        inputProperty = messages.Where(x => x.Name == matches.Groups[4].Value)
                                            .FirstOrDefault();

                                    if (messages.Any(x => x.Name == matches.Groups[6].Value))
                                        outputProperty = messages.Where(x => x.Name == matches.Groups[6].Value)
                                                .FirstOrDefault();

                                    serviceDefinition.EndpointDefinitions.Add(new EndpointDefinition
                                    {
                                        Name = matches.Groups[2].Value,
                                        Input = inputProperty,
                                        Output = outputProperty
                                    });
                                }
                            }
                        }

                        services.Add(serviceDefinition);
                    }
                }
            }
        }
    }
}
