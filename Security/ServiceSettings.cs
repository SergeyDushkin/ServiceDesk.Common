using System;

namespace servicedesk.Common.Security
{
    public class ServiceSettings
    {
        public TimeSpan? CacheExpiry { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayMilliseconds { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
    }
}