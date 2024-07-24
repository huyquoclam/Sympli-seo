using Sympli.Application.Common;
using Sympli.Search.Google.Services;

namespace Sympli.UnitTests.Services;

[TestFixture]
public class GoogleSearchServiceTests
{
    private GoogleSearchService _googleSearchService;

    [SetUp]
    public void SetUp()
    {
        DummyCacheManager cacheManager = new DummyCacheManager();
        ApplicationOptions applicationOptions = new ApplicationOptions()
        {
            EngineSearchCachingInSeconds = 60,
            MaxSearchResults = 100
        };
        // Initialize the BingSearchService instance
        _googleSearchService = new GoogleSearchService(cacheManager, applicationOptions);
    }

    [Test]
    public async Task Google_Search_SympliSEO_ReturnsSearchResults()
    {
        // Arrange

        // Act
        var results = await _googleSearchService.GetUrlPositionsResults();

        // Assert
       Assert.IsNotNull(results);
    }

    [Test]
    public async Task Google_Search_WithQuery_ReturnsEmptyResults()
    {
        // Arrange
        string keyword = "google";
        string url = "https://www.abcxzy.com";

        // Act
        var results = await _googleSearchService.GetUrlPositionsResultsWithCustomKeywordsAndUrl(keyword, url);

        // Assert
        Assert.IsNotNull(results);
        Assert.AreEqual(0, results.Count);
    }
    [Test]
    public async Task Google_Search_WithQuery_ReturnsResults()
    {
        // Arrange
        string keyword = "google";
        string url = "https://www.google.com";

        // Act
        var results = await _googleSearchService.GetUrlPositionsResultsWithCustomKeywordsAndUrl(keyword, url);

        // Assert
        Assert.IsNotNull(results);
        Assert.IsTrue(results.Count > 0);
    }
}
