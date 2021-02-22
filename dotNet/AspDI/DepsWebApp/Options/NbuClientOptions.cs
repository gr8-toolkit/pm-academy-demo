using System;

namespace DepsWebApp.Options
{
    public class NbuClientOptions
    {
        public string BaseAddress { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(BaseAddress) &&
                               Uri.TryCreate(BaseAddress, UriKind.Absolute, out _);
    }
}
