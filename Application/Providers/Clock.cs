using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeCMS.Application.Providers
{
    class Clock : IClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
