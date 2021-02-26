using System;

namespace DepsWebApp.Options
{
    /// <summary>
    /// Caching options.
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Default rates cache lifetime.
        /// </summary>
        public TimeSpan? RatesCacheLifeTime{ get; set; }
    }
}
