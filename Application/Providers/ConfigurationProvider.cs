using System.Configuration;

namespace CodeKinden.OrangeCMS.Application.Providers
{
    public class ConfigurationProvider
    {
        private static string GetAppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null) throw new ApplicationConfigurationException(key);
            return value;
        }

        public class System
        {
            public static string Username => GetAppSetting("system.username");

            public static string Password => GetAppSetting("system.password");

            public static string Email => GetAppSetting("system.email");
        }

        public static string ConnectionString => ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
    }
}
