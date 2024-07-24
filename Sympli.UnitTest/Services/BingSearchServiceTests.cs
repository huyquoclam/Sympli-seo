using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.UnitTest.Services;

internal class BingSearchServiceTests
{
    public class BingSearchServiceTests
    {
        private readonly BingSearchService _bingSearchService;
        private readonly Mock<ICacheManager> _mockCacheManager;
        private readonly Mock<IOptions<ApplicationOptions>> _mockOptions;

        public BingSearchServiceTests()
        {
            _mockCacheManager = new Mock<ICacheManager>();
            _mockOptions = new Mock<IOptions<ApplicationOptions>>();
            _bingSearchService = new BingSearchService(Mock.Of<IHttpClientFactory>(), _mockCacheManager.Object, _mockOptions.Object);
        }

        [Fact]
        public async Task GetUrlPositionsResultsWithCustomKeywordsAndUrl_ShouldReturnUrlPositions()
        {
            // Arrange
            string keywords = "test";
            string url = "example.com";
            List<int> expectedPositions = new List<int> { 1, 2, 3 };
            _mockCacheManager.Setup(x => x.GetOrSetAsync<List<int>>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Func<Task<List<int>>>>()))
                .ReturnsAsync(expectedPositions);

            // Act
            List<int> actualPositions = await _bingSearchService.GetUrlPositionsResultsWithCustomKeywordsAndUrl(keywords, url);

            // Assert
            Assert.Equal(expectedPositions, actualPositions);
        }

        [Fact]
        public async Task GetUrlPositionsResults_ShouldReturnUrlPositions()
        {
            // Arrange
            List<int> expectedPositions = new List<int> { 1, 2, 3 };
            _mockCacheManager.Setup(x => x.GetOrSetAsync<List<int>>(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Func<Task<List<int>>>>()))
                .ReturnsAsync(expectedPositions);

            // Act
            List<int> actualPositions = await _bingSearchService.GetUrlPositionsResults();

            // Assert
            Assert.Equal(expectedPositions, actualPositions);
        }
    }
}
