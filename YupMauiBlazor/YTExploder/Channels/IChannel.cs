using System.Collections.Generic;
using YupMauiBlazor.YTExploder.Common;

namespace YupMauiBlazor.YTExploder.Channels;

/// <summary>
/// Properties shared by channel metadata resolved from different sources.
/// </summary>
public interface IChannel
{
    /// <summary>
    /// Channel ID.
    /// </summary>
    ChannelId Id { get; }

    /// <summary>
    /// Channel URL.
    /// </summary>
    string Url { get; }

    /// <summary>
    /// Channel title.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Channel thumbnails.
    /// </summary>
    IReadOnlyList<Thumbnail> Thumbnails { get; }
}