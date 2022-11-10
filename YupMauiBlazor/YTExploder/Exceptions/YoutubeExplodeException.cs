using System;

namespace YupMauiBlazor.YTExploder.Exceptions
{ 

/// <summary>
/// Exception thrown within <see cref="YUPBlazor.YTExploder"/>.
/// </summary>
public class YoutubeExplodeException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="YUPBlazor.YTExploderException"/>.
    /// </summary>
    /// <param name="message"></param>
    public YoutubeExplodeException(string message) : base(message)
    {
    }
}
}