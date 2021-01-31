namespace RequestProcessor.App.Models
{
    /// <summary>
    /// Request methods.
    /// Methods are projected to HTTP methods.
    /// </summary>
    internal enum RequestMethod
    {
        Undefined = 0,
        Get = 1,
        Post = 2,
        Put = 3,
        Patch = 4,
        Delete = 5
    }
}
