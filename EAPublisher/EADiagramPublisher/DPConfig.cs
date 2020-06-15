using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EADiagramPublisher
{
    public static class DPConfig
    {
        public static KeyValueConfigurationCollection AppSettings
        {
            get
            {
                return ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location).AppSettings.Settings;
            }
        }
    }
}
