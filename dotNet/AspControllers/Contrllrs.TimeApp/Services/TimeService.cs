using System;

namespace Contrllrs.TimeApp.Services
{
    internal class TimeService : ITimeService
    {
        public string GetServerTime()
        {
            return DateTimeOffset.UtcNow.ToString("O");
        }
    }
}
