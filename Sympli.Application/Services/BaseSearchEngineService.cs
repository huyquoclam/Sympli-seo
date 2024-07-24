using Sympli.Application.Common;
using System.Text.RegularExpressions;

namespace Sympli.Application.Services;

public abstract class BaseSearchEngineService()
{
    public abstract string DefaultSympliKeywords { get; }
    public abstract string DefaultSympliUrl { get; }
    protected abstract string DefaultSearchPattern { get; }
    protected abstract string SearchEngineUrl { get; }
    protected abstract string OffsetParams { get; }
    protected abstract string CountParams { get; }
    protected virtual int MaxSearchResults { get; } = 100;

    protected virtual async Task<List<int>> GetUrlPositionsResults(string keywords, string url)
    {
        List<int> positions = [];

        int currentResultCount = 0;
        int startIndex = 0;

        string searchUrl = SanitizeUrl(string.IsNullOrEmpty(url) ? DefaultSympliUrl : url);
        string searchKeywords = string.IsNullOrEmpty(keywords) ? DefaultSympliKeywords : keywords;

        while (currentResultCount < MaxSearchResults)
        {
            string requestUri = BuildRequestUri(searchKeywords, startIndex);

            // Perform the search on search engine
            string content = string.Empty;

            try
            {
                content = await FetchContentWithRetry(requestUri);
                // content = await HttpCallHelper.GetAsync(requestUri);
            }
            catch (Exception ex)
            {
                // Log the exception and return the positions found so far
                Console.WriteLine($"Error while fetching content: {ex.Message}");
                return positions;
            }

            // Extract the search results
            Regex regex = new(DefaultSearchPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(content);

            int position = startIndex + 1;
            foreach (Match match in matches)
            {
                // Check if the search result contains the URL
                if (match.Value.Contains(searchUrl))
                {
                    positions.Add(position);
                }

                position++;
                currentResultCount++;

                if (currentResultCount >= MaxSearchResults) // reach the limit
                {
                    break;
                }
            }

            if (matches.Count == 0)
            {
                break; // No more results available
            }

            startIndex += matches.Count;
        }

        return positions;
    }

    private async Task<string> FetchContentWithRetry(string requestUri)
    {
        return await RetryPolicy.ExecuteWithRetryAsync(async () =>
        {
            return await HttpCallHelper.GetAsync(requestUri);
        }, maxRetryAttempts: 3, delay: TimeSpan.FromSeconds(2));
    }

    private string BuildRequestUri(string searchKeywords, int startIndex)
    {
        return $"{SearchEngineUrl}{searchKeywords}&{CountParams}={MaxSearchResults}&{OffsetParams}={startIndex + 1}";
    }

    private string SanitizeUrl(string url)
    {
        // Trim and replace http:// or https:// from url if present
        // This is to ensure that the URL is in the correct format for comparison
        // Trim any trailing slashes
        return url
            .Trim()
            .Replace("http://www.", "")
            .Replace("https://www.", "")
            .TrimEnd('/');
    }
}
