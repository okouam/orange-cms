using System;

namespace CodeKinden.OrangeCMS.Application.Exceptions
{
    public class ApplicationConfigurationException : Exception
    {
        public override string Message
        {
            get
            {
                return string.Format("The application setting {0} has not been configured. Please check the web.config or app.config.", key);
            }
        }

        public ApplicationConfigurationException(string key)
        {
            this.key = key;
        }

        private readonly string key;
    }
}