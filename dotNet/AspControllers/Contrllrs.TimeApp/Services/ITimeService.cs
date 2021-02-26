namespace Contrllrs.TimeApp.Services
{
    /// <summary>
    /// Time service.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Get current server time
        /// </summary>
        /// <returns>Return time string.</returns>
        string GetServerTime();
    }
}
