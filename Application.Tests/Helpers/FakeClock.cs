using System;
using CodeKinden.OrangeCMS.Domain.Providers;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers
{
    class FakeClock : IClock
    {
        private DateTime now;

        public void Set(DateTime currentDateTime)
        {
            this.now = currentDateTime;
        }

        public DateTime Now()
        {
            var current = now;
            now = now.AddMinutes(1);
            return current;
        }
    }
}
