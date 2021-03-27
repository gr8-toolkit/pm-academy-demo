namespace AuthWebApps.AuthServices.Models
{
    /// <summary>
    /// Account model.
    /// </summary>
    internal readonly struct Account
    {
        /// <summary>
        /// Public id.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Password hash.
        /// </summary>
        public int PasswordHash { get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public Account(int id, int passwordHash)
        {
            Id = id;
            PasswordHash = passwordHash;
        }
    }
}
