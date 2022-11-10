using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YupMauiBlazor.YTExploder.Bridge;

namespace YupMauiBlazor.YTExploder.Videos.Streams;

internal class StreamController : VideoController
{
    public StreamController(HttpClient http)
        : base(http)
    {
    }

    public async ValueTask<DashManifestExtractor> GetDashManifestAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        var raw = await SendHttpRequestAsync(url, cancellationToken);
        return DashManifestExtractor.Create(raw);
    }
}