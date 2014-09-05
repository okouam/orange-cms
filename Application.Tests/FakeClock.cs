using System;
using OrangeCMS.Application.Providers;

namespace OrangeCMS.Application.Tests
{
    class FakeClock : IClock
    {
        private DateTime now;

        public void Set(DateTime now)
        {
            this.now = now;
        }

        public DateTime Now()
        {
            var current = now;
            now = now.AddMinutes(1);
            return current;
        }
    }
}
