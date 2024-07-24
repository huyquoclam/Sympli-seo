using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Common;

public class HttpCallHelper
{
    public static async Task<string> GetAsync(string url)
    {
        // Add a unique query parameter to the URL to bypass any cache
        var uniqueUrl = $"{url}&_={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);

        // Add headers to mimic a browser request
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        client.DefaultRequestHeaders.Connection.Add("keep-alive");
        client.DefaultRequestHeaders.Host = new Uri(url).Host;
        client.DefaultRequestHeaders.Referrer = new Uri(url);

        // Add headers to prevent caching
        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true, MaxAge = TimeSpan.Zero };
        client.DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));

        // Clear cookies between requests
        handler.CookieContainer = new System.Net.CookieContainer();

        var response = await client.GetAsync(uniqueUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return content;
    }
}
