using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestableWebApi.Common
{
    internal class AppInfo
    {
        private static DateTime _appStartTime = DateTime.UtcNow;

        public int UptimeSeconds => (int)(DateTime.UtcNow - _appStartTime).TotalSeconds;
    }
}
