using RestGrpcProxy.Models;

namespace RestGrpcProxy.Generators
{
    public class ObjectGenerator
    {
        public static List<string> Generate(List<MessageDefinition> messages)
        {
            var classTemplate = File.ReadAllText(Path.Combine("Templates", "ClassTemplate.txt"));
            var classList = new List<string>();

            foreach(var message in messages)
            {
                var classSource = classTemplate.Replace("$NAME", message.Name);

                var propertyString = "";
                foreach(var property in message.Properties)
                {
                    propertyString += "\t\tpublic " + GetCsharpTypeName(property.Value) +
                        " " + property.Key + " {get; set;}\n";
                }

                classSource = classSource.Replace("$PROPERTIES", propertyString);
                classList.Add(classSource);
            }

            return classList;
        }

        private static string GetCsharpTypeName(string grpcTypeName)
        {
            if(_grpcToCsharpTypeNameMap.ContainsKey(grpcTypeName))
                return _grpcToCsharpTypeNameMap[grpcTypeName];

            return "object";
        }

        private static Dictionary<string, string> _grpcToCsharpTypeNameMap = new Dictionary<string, string>
        {
            { "string", "string" },
            { "int32", "Int32" },
            { "int64", "Int64" },
        };
    }
}
