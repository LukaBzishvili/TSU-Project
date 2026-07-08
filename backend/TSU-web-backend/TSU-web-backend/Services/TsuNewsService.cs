using System.Text.RegularExpressions;
using TSU_web_backend.Dtos;

namespace TSU_web_backend.Services;

public partial class TsuNewsService(HttpClient httpClient, ILogger<TsuNewsService> logger) : ITsuNewsService
{
    private static readonly string[] SourceUrls =
    [
        "https://www.computing.tsu.ge/en/news",
        "https://computing.tsu.ge/en/news",
        "https://computing.tsu.ge/en"
    ];

    private const string SourceUrl = "https://www.computing.tsu.ge/en/news";
    private const string SourceBaseUrl = "https://computing.tsu.ge";

    public async Task<IReadOnlyList<ExternalNewsItemDto>> GetLatestNewsAsync(CancellationToken cancellationToken = default)
    {
        foreach (var url in SourceUrls)
        {
            try
            {
                var html = await httpClient.GetStringAsync(url, cancellationToken);
                var items = ParseNewsItems(html);

                if (items.Count > 0)
                {
                    return items;
                }

                logger.LogWarning("No TSU Computer Science news items were parsed from {SourceUrl}", url);
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Failed to fetch TSU Computer Science news from {SourceUrl}", url);
            }
        }

        return GetFallbackItems();
    }

    private static string BuildAbsoluteUrl(string href)
    {
        if (string.IsNullOrWhiteSpace(href))
        {
            return SourceUrl;
        }

        if (Uri.TryCreate(href, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        return $"{SourceBaseUrl.TrimEnd('/')}/{href.TrimStart('/')}";
    }

    private static string CleanText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var withoutTags = Regex.Replace(value, "<.*?>", " ");
        var decoded = System.Net.WebUtility.HtmlDecode(withoutTags);
        var cleaned = Regex.Replace(decoded, @"\s+", " ").Trim();
        cleaned = cleaned
            .Replace(" ", " ")
            .Replace("####", " ")
            .Replace("###", " ")
            .Replace("##", " ")
            .Trim();

        return cleaned;
    }

    private static IReadOnlyList<ExternalNewsItemDto> ParseNewsItems(string html)
    {
        var items = ParseDetailedCards(html);

        if (items.Count == 0)
        {
            items = ParseHeadlineDatePairs(html);
        }

        return items
            .Where(item => !string.IsNullOrWhiteSpace(item.Title))
            .DistinctBy(item => item.Title)
            .Take(12)
            .ToList();
    }

    private static List<ExternalNewsItemDto> ParseDetailedCards(string html)
    {
        var cards = NewsCardRegex().Matches(html);
        var items = new List<ExternalNewsItemDto>();

        foreach (Match card in cards)
        {
            var title = CleanText(card.Groups["title"].Value);
            var date = CleanText(card.Groups["date"].Value);
            var summary = NormalizeSummary(CleanText(card.Groups["summary"].Value), title);
            var href = CleanText(card.Groups["href"].Value);

            if (string.IsNullOrWhiteSpace(title))
            {
                continue;
            }

            items.Add(new ExternalNewsItemDto
            {
                Title = title,
                Date = date,
                Summary = summary,
                Url = BuildAbsoluteUrl(href),
            });
        }

        return items;
    }

    private static List<ExternalNewsItemDto> ParseHeadlineDatePairs(string html)
    {
        var items = new List<ExternalNewsItemDto>();
        var matches = HeadlineDateRegex().Matches(html);

        foreach (Match match in matches)
        {
            var title = CleanText(match.Groups["title"].Value);
            var date = CleanText(match.Groups["date"].Value);
            var href = CleanText(match.Groups["href"].Value);

            if (string.IsNullOrWhiteSpace(title))
            {
                continue;
            }

            items.Add(new ExternalNewsItemDto
            {
                Title = title,
                Date = date,
                Summary = DefaultSummary,
                Url = BuildAbsoluteUrl(href),
            });
        }

        return items;
    }

    private static IReadOnlyList<ExternalNewsItemDto> GetFallbackItems()
    {
        return
        [
            new ExternalNewsItemDto
            {
                Title = "TSU STUDENTS WILL TRAVEL TO FRENCH LANGUAGE COURSES",
                Date = "14 June 2025",
                Summary = "Students of the Computer Science Department at TSU were selected for a one-month French language course.",
                Url = "https://www.computing.tsu.ge/en/news/77"
            },
            new ExternalNewsItemDto
            {
                Title = "First graduate of the Computer Science (Georgian-French) Double Diploma Bachelor's Program",
                Date = "15 May 2025",
                Summary = "An official update from the TSU Computer Science department about the first graduate of the Georgian-French double diploma program.",
                Url = "https://www.computing.tsu.ge/en/news"
            },
            new ExternalNewsItemDto
            {
                Title = "A conference on \"Gender Dimensions of Cybersecurity\" was held.",
                Date = "19 June 2024",
                Summary = "TSU Computer Science shared a department news item about a conference focused on cybersecurity and gender dimensions.",
                Url = "https://www.computing.tsu.ge/en/news"
            }
        ];
    }

    private static string NormalizeSummary(string summary, string title)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return DefaultSummary;
        }

        var cleaned = summary.Trim();

        if (cleaned.StartsWith(title, StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[title.Length..].TrimStart(' ', '-', ':');
        }

        var noisyFragments = new[]
        {
            "Image",
            "Useful links",
            "contact information",
            "Details",
            "Department of Computer Science",
            "Dean",
            "Head of direction",
            "href=",
            "src=",
            "class=",
            "style=",
            "alt=",
            "width=",
            "height=",
            "loading=",
            "decoding=",
            "data-",
            "breadcrumb",
            "navbar",
            "dropdown",
            "container",
            "btn-",
            "aria-"
        };

        if (noisyFragments.Any(fragment => cleaned.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
        {
            return DefaultSummary;
        }

        if (LooksLikeMarkupNoise(cleaned))
        {
            return DefaultSummary;
        }

        if (cleaned.Length > 220)
        {
            cleaned = cleaned[..220];
            var lastSpace = cleaned.LastIndexOf(' ');
            if (lastSpace > 80)
            {
                cleaned = cleaned[..lastSpace];
            }

            cleaned += "...";
        }

        return string.IsNullOrWhiteSpace(cleaned) ? DefaultSummary : cleaned;
    }

    private const string DefaultSummary = "Open the official TSU Computer Science news page for full details.";

    private static bool LooksLikeMarkupNoise(string value)
    {
        var equalsCount = value.Count(character => character == '=');
        var quoteCount = value.Count(character => character == '"' || character == '\'');

        return equalsCount >= 2
            || quoteCount >= 4
            || value.Contains("/>", StringComparison.Ordinal)
            || value.Contains("http://", StringComparison.OrdinalIgnoreCase)
            || value.Contains("https://", StringComparison.OrdinalIgnoreCase)
            || Regex.IsMatch(value, @"\b[a-z-]+=""[^""]+""", RegexOptions.IgnoreCase)
            || Regex.IsMatch(value, @"\b[a-z-]+='[^']+'", RegexOptions.IgnoreCase);
    }

    [GeneratedRegex(
        "<a[^>]*href=\"(?<href>[^\"]+)\"[^>]*>\\s*(?:<img[^>]*>\\s*)?(?:<[^>]+>\\s*)*(?<title>.*?)\\s*</a>\\s*(?:<[^>]+>\\s*)*(?<date>\\d{1,2}\\s+[A-Za-z]+\\s+\\d{4}|\\d{1,2}/\\d{1,2}/\\d{4}|\\d{1,2}\\s+[A-Za-z]+\\s+\\d{2,4})\\s*(?:<[^>]+>\\s*)*(?<summary>.*?)(?=(?:<a[^>]*href=)|(?:</article>)|(?:<img[^>]*>)|(?:<section)|(?:<div class=\"pagination\")|$)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex NewsCardRegex();

    [GeneratedRegex(
        "href=\"(?<href>[^\"]*/en/news/\\d+[^\"]*)\"[^>]*>\\s*(?:<img[^>]*>\\s*)*(?:<h\\d[^>]*>\\s*)?(?<title>[^<]+?)\\s*(?:</h\\d>)?\\s*</a>.*?(?<date>\\d{1,2}\\s+[A-Za-z]+\\s+\\d{4})",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex HeadlineDateRegex();
}
