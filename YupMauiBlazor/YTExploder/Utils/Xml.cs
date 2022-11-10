using System.Xml.Linq;
using YupMauiBlazor.YTExploder.Utils.Extensions;

namespace YupMauiBlazor.YTExploder.Utils;

internal static class Xml
{
    public static XElement Parse(string source) =>
        XElement.Parse(source, LoadOptions.PreserveWhitespace).StripNamespaces();
}