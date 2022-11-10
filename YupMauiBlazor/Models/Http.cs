using System.Net.Http.Headers;

namespace YupMauiBlazor.Models
{
    public class Http
    {
        public static HttpClient Client { get; } = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7190"),
            DefaultRequestHeaders =
            {
                UserAgent =
                {
                    new ProductInfoHeaderValue("YupExtension", "1.0.0.10")
                }
            }
        };
    }
}
