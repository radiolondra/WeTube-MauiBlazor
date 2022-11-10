using System.Net.Http;
using YupMauiBlazor.YTExploder.Channels;
using YupMauiBlazor.YTExploder.Playlists;
using YupMauiBlazor.YTExploder.Search;
using YupMauiBlazor.YTExploder.Utils;
using YupMauiBlazor.YTExploder.Videos;

namespace YupMauiBlazor.YTExploder;

/// <summary>
/// Client for interacting with YouTube.
/// </summary>
public class YoutubeClient
{
    /// <summary>
    /// Operations related to YouTube videos.
    /// </summary>
    public VideoClient Videos { get; }

    /// <summary>
    /// Operations related to YouTube playlists.
    /// </summary>
    public PlaylistClient Playlists { get; }

    /// <summary>
    /// Operations related to YouTube channels.
    /// </summary>
    public ChannelClient Channels { get; }

    /// <summary>
    /// Operations related to YouTube search.
    /// </summary>
    public SearchClient Search { get; }

    /// <summary>
    /// Initializes an instance of <see cref="YoutubeClient"/>.
    /// </summary>
    public YoutubeClient(HttpClient http)
    {
        Videos = new VideoClient(http);
        Playlists = new PlaylistClient(http);
        Channels = new ChannelClient(http);
        Search = new SearchClient(http);
    }

    /// <summary>
    /// Initializes an instance of <see cref="YoutubeClient"/>.
    /// </summary>
    public YoutubeClient() : this(Http.Client)
    {
    }
}