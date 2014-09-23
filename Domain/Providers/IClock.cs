using System;

namespace CodeKinden.OrangeCMS.Domain.Providers
{
    public interface IClock
    {
        DateTime Now();
    }
}