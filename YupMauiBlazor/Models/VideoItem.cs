using System.Runtime.CompilerServices;
using YupMauiBlazor.YTExploder.Channels;
using YupMauiBlazor.YTExploder.Common;
using YupMauiBlazor.YTExploder.Playlists;
using YupMauiBlazor.YTExploder.Search;
using YupMauiBlazor.YTExploder.Videos;
using YupMauiBlazor.YTExploder;
using YupMauiBlazor.YTExploder.Videos.Streams;
using System.Drawing;
using Microsoft.AspNetCore.Components;

namespace YupMauiBlazor.Models
{
    public class VideoItem
    {
        private static HttpClient httpClient = new();
        private static readonly YoutubeClient _youtube = new(Http.Client);

        public static IAsyncEnumerator<Batch<ISearchResult>> BatchEnumerator { get; set; }
        public static IAsyncEnumerator<PlaylistVideo> ListEnumerator { get; set; }

        public string Channel_Name { get; set; }
        public string Channel_Id { get; set; }

        public string CoverUrl { get; set; }

        public string ImageJpgDataUrl { get; set; }
        public string TitleUrl { get; set; }
        public string Title { get; set; }
        public int VideoIndex { get; set; }
        public bool VideoSelectedForDownload { get; set; }
        public bool AudioSelectedForDownload { get; set; }

        public string EphemeralVideoUrl { get; set; }        

        public string OnOffLine { get; set; }
        public TimeSpan? Duration { get; set; }
        public string DurationUI { get; set; }        
        public Blazorise.IconName EphemeralPlayIcon { get; set; }

        public string StyleVisible { get; set; }
        public string StyleInvisible { get; set; }


        public VideoItem(
            string channel_name = "",
            string channel_id = "",
            string title = "",
            string coverUrl = "",
            string titleUrl = "",
            TimeSpan? duration = null)
        {
            Channel_Name = channel_name;
            Channel_Id = channel_id;
            Title = title;
            CoverUrl = coverUrl;
            TitleUrl = titleUrl;
            EphemeralVideoUrl = String.Empty;
            Duration = duration;
            OnOffLine = "Off-Line";

            //EphemeralPlayIconColor = MudBlazor.Color.Error;
            EphemeralPlayIcon = Blazorise.IconName.Play;
            //ImageVisible = Blazorise.Visibility.Visible;
            //VideoVisible = Blazorise.Visibility.Invisible;


        }

        /*
        public static byte[] ImageToByteArray(string imageName)
        {
            //Initialize a file stream to read the image file
            FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();

            return imgByteArr;
        }

        public async Task<Stream> LoadCoverBitmapAsync(CancellationToken token)
        {
            byte[] data;
            try
            {
                data = await httpClient.GetByteArrayAsync(CoverUrl, token);
            }
            catch (Exception ex)
            {
                data = ImageToByteArray(Utilities.Settings.YupImageNotAvailable);
            }

            return new MemoryStream(data);

        }
        */


        public static async IAsyncEnumerable<VideoItem> SearchAsync(bool getEnumerator, string searchTerm, [EnumeratorCancellation] CancellationToken token)
        {

            var parsedQuery = ParseQuery(searchTerm!);
            Console.WriteLine($"Query:{parsedQuery.Value}");

            // Video
            if (parsedQuery.Kind == QueryKind.Video)
            {
                Video vid = null;
                try
                {
                    vid = await _youtube.Videos.GetAsync(parsedQuery.Value);
                }
                catch { }

                if (vid != null)
                {
                    yield return new VideoItem(
                        vid.Author.ChannelTitle,
                        vid.Author.ChannelId,
                        vid.Title,
                        vid.Thumbnails.TryGetWithHighestResolution().Url,
                        vid.Url,
                        vid.Duration);
                }

            }

            //CHANNEL or PLAYLIST
            if (parsedQuery.Kind == QueryKind.Channel || parsedQuery.Kind == QueryKind.Playlist)
            {
                if (getEnumerator)
                {
                    if (parsedQuery.Kind == QueryKind.Channel)
                    {
                        try
                        {
                            ListEnumerator = _youtube.Channels.GetUploadsAsync(parsedQuery.Value, token).GetAsyncEnumerator();
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            ListEnumerator = _youtube.Playlists.GetVideosAsync(parsedQuery.Value, token).GetAsyncEnumerator();
                        }
                        catch { }
                    }
                }

                if (ListEnumerator != null)
                {
                    int howmany = 0;
                    while (howmany++ < Utilities.Settings.MAX_ITEMS_CHUNK)
                    {
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                            break;
                        }

                        // Playlist or channel could be private so that inaccessible and will be jumped
                        bool avail = false;
                        try
                        {
                            avail = await ListEnumerator.MoveNextAsync().ConfigureAwait(false);
                        }
                        catch { }

                        if (avail)
                        {
                            var item = ListEnumerator.Current;

                            yield return new VideoItem(
                                item.Author.ChannelTitle,
                                item.Author.ChannelId,
                                item.Title,
                                item.Thumbnails.TryGetWithHighestResolution().Url,
                                item.Url,
                                item.Duration
                                );
                        }

                    }
                }
            }

            // SEARCH
            if (parsedQuery.Kind == QueryKind.Search)
            {
                if (getEnumerator)
                {
                    try
                    {
                        BatchEnumerator = _youtube.Search.GetResultBatchesAsync(parsedQuery.Value, SearchFilter.Video, token).GetAsyncEnumerator();
                    }
                    catch (Exception ex) { Console.WriteLine($"Error getting IEnumerator {ex.Message}"); }
                }

                if (BatchEnumerator != null)
                {
                    if (await BatchEnumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        var videos = BatchEnumerator.Current;
                        if (videos != null)
                        {
                            foreach (var video in videos.Items)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    token.ThrowIfCancellationRequested();
                                    break;
                                }

                                if (video is VideoSearchResult videoSearchResult)
                                {
                                    yield return new VideoItem(
                                    videoSearchResult.Author.ChannelTitle,
                                    videoSearchResult.Author.ChannelId,
                                    videoSearchResult.Title,
                                    videoSearchResult.Thumbnails.TryGetWithHighestResolution().Url,
                                    videoSearchResult.Url,
                                    videoSearchResult.Duration
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Query ParseQuery(string query)
        {

            query = query.Trim();

            // Playlist
            var playlistId = PlaylistId.TryParse(query);
            if (playlistId is not null)
            {
                return new Query(QueryKind.Playlist, playlistId.Value);
            }

            // Video
            var videoId = VideoId.TryParse(query);
            if (videoId is not null)
            {
                return new Query(QueryKind.Video, videoId.Value);
            }

            // Channel
            var channelId = ChannelId.TryParse(query);
            if (channelId is not null)
            {
                return new Query(QueryKind.Channel, channelId.Value);
            }

            // Search
            {
                return new Query(QueryKind.Search, query);
            }

        }

        /// <summary>
        /// Load video thumbnail
        /// </summary>
        /// <returns></returns>
        //public async Task LoadCover(CancellationToken token)
        public void LoadCover(CancellationToken token)
        {
            if (this.CoverUrl == null || this.CoverUrl == String.Empty)
                this.CoverUrl = "Assets/covernotfound.png";
        }        

        public async Task GetManifestInfo(CancellationToken token)
        {
            var youtube = new YoutubeClient();

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(TitleUrl, token);
            EphemeralVideoUrl = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality().Url;
            OnOffLine = EphemeralVideoUrl == string.Empty || EphemeralVideoUrl == null ? "Off-Line" : "On-Line";
            
        }
    }
}
