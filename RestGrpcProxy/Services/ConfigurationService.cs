using RestGrpcProxy.Models;

namespace RestGrpcProxy.Services
{
    public class ConfigurationService
    {
        private static Configuration _config;

        public ConfigurationService()
        {
            LoadConfiguration("config.json");
        }

        public Configuration Config => _config;

        private void LoadConfiguration(string name)
        {
            if (_config == null)
            {
                var configFile = File.ReadAllText(name);
                _config = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(configFile);
            }
        }

        private void SaveConfiguration(string name)
        {
            if(_config != null)
            {
                var configString = Newtonsoft.Json.JsonConvert.SerializeObject(_config, 
                    Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(name, configString);
            }
        }
    }
}
