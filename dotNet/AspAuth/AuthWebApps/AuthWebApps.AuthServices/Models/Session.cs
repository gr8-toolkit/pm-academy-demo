using System;

namespace AuthWebApps.AuthServices.Models
{
    /// <summary>
    /// Session.
    /// </summary>
    /// <typeparam name="T">Session data type.</typeparam>
    public class Session<T>
    {
        /// <summary>
        /// Sessions data.
        /// </summary>
        public T SessionData { get; set; }
        
        /// <summary>
        /// Session owner.
        /// </summary>
        public int AccountId { get; set; }
        
        /// <summary>
        /// Session expiration.
        /// </summary>
        public DateTime Expiration { get; set; }
        
        /// <summary>
        /// Constructor. 
        /// </summary>
        public Session()
        {
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        public Session(T sessionData, int accountId, DateTime expiration)
        {
            SessionData = sessionData;
            AccountId = accountId;
            Expiration = expiration;
        }
    }
}
