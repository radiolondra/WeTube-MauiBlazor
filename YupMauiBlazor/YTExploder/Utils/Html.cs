using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace YupMauiBlazor.YTExploder.Utils;

internal static class Html
{
    private static readonly HtmlParser HtmlParser = new();

    public static IHtmlDocument Parse(string source) => HtmlParser.ParseDocument(source);
}