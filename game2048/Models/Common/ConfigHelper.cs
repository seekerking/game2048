using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Models.Common
{
    public class ConfigHelper
    {    
        public ConfigHelper(IConfiguration configuration)
        {
             Config=configuration.Get<Config>();
              StaticConfig = Config;

        }

        public static Config StaticConfig { get; set; }

        public Config Config { get; set; }

        public static IConfiguration GetConfiguration(string configName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configName, optional: true, reloadOnChange: true);
            return builder.Build();

        }


    }
}

