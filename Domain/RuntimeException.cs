using System;

namespace CodeKinden.OrangeCMS.Domain
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message, params object[] args) : base(String.Format(message, args))
        {
        }
    }
}
