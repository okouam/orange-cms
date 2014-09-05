using System;

namespace OrangeCMS.Application.Providers
{
    public interface IClock
    {
        DateTime Now();
    }
}