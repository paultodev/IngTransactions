using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IngTransactions.Util
{
    public class AppConfiguration
    {
        // Test
        public IConfigurationRoot configuration;

        private IConfigurationRoot GetIConfigurationRoot()
        {
            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
            }
            catch (Exception e)
            {
                // Error loading the application configuration file 
            }

            return configuration;
        }


        public AppConfiguration GetApplicationConfiguration()
        {
            var configuration = new AppConfiguration();

            var iConfig = GetIConfigurationRoot();

            iConfig
                .GetSection("Certificates")
                .Bind(configuration);

            return configuration;
        }

        public string GetPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        }
    }

}
