namespace RequestProcessor.App.Models
{
    /// <summary>
    /// Response abstraction.
    /// </summary>
    internal interface IResponse
    {
        /// <summary>
        /// Indicates that request was handled.
        /// HTTP-status code <see cref="Code"/> doesn't matter.
        /// <c>false</c> value indicates that request wasn't processed (usually because of timeout).
        /// </summary>
        public bool Handled { get; }
        
        /// <summary>
        /// Response HTTP-status code.
        /// </summary>
        public int Code { get; }
        
        /// <summary>
        /// Response content.
        /// </summary>
        public string Content { get; }
    }
}