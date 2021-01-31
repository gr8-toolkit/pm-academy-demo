namespace RequestProcessor.App.Models
{
    /// <summary>
    /// Request options.
    /// </summary>
    internal interface IRequestOptions
    {
        /// <summary>
        /// Optional friendly name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Request address.
        /// Should be valid Uri.
        /// </summary>
        string Address { get; }
        
        /// <summary>
        /// Request method.
        /// </summary>
        RequestMethod Method { get; }

        /// <summary>
        /// Request content type.
        /// Can be <c>null</c> when <see cref="Body"/> is null.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Request content.
        /// Optional property.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Indicates that options are valid.
        /// </summary>
        bool IsValid { get; }
    }
}
