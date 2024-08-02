using Microsoft.AspNetCore.Mvc;
using Sympli.Application.Common;
using Sympli.Application.CQRS.Messaging;
using Sympli.Application.Features.KeywordSearch.Contracts.Queries;
using Sympli.Application.Features.KeywordSearch.Contracts.Views;
using Sympli.Application.Services;
using Sympli.WebAPI.Filters;

namespace Sympli.WebAPI.Controllers
{
    [ApiController]
    [Route("keyword-search")]
    public class KeywordSearchController(
        ILogger<KeywordSearchController> logger, 
        IKeywordSearchServiceFactory keywordSearchServiceFactory,
        IMessageDispatcher dispatcher
        ) : ControllerBase
    {
        private readonly ILogger<KeywordSearchController> _logger = logger;
        private readonly IKeywordSearchServiceFactory _keywordSearchServiceFactory = keywordSearchServiceFactory;
        private readonly IMessageDispatcher _dispatcher = dispatcher;

        /// <summary>
        /// Gets Sympli SEO results from google search
        /// </summary>
        /// <returns></returns>
        [HttpGet("google/sympli-seo-results", Name = "GetSympliSEOResultsFromGoogleSearch")]
        [ProducesResponseType(typeof(SEOResultsModel), StatusCodes.Status200OK)]
        [ConcurrentRateLimiterFilter]
        public async Task<IActionResult> GetSympliSEOResultsFromGoogleSearch()
        {
            var result = await _dispatcher.DispatchAsync<SEOResultsModel>(new GetSympliSEOResultsFromGoogleSearchQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets SEO results with custom keyword and URL from google search
        /// </summary>
        /// <returns></returns>
        [HttpGet("google/seo-results", Name = "GetSEOResultsFromGoogleSearch")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSEOResultsFromGoogleSearch([FromQuery] string keyword, [FromQuery] string url)
        {
            var result = await _dispatcher.DispatchAsync<SEOResultsModel>(new GetSEOResultsFromGoogleSearchQuery(keyword, url));
            return Ok(result);
        }

        /// <summary>
        /// Gets Sympli SEO results from bing search
        /// </summary>
        /// <returns></returns>
        [HttpGet("bing/sympli-seo-results", Name = "GetSympliSEOResultsFromBingSearch")]
        [ProducesResponseType(typeof(SEOResultsModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSympliSEOResultsFromBingSearch()
        {
            var result = await _dispatcher.DispatchAsync<SEOResultsModel>(new GetSympliSEOResultsFromBingSearchQuery());
            return Ok(result);
        }

        /// <summary>
        /// Gets SEO results with custom keyword and URL from bing search
        /// </summary>
        /// <returns></returns>
        [HttpGet("bing/seo-results", Name = "GetSEOResultsFromBingSearch")]
        [ProducesResponseType(typeof(SEOResultsModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSEOResultsFromBingSearch([FromQuery] string keyword, [FromQuery] string url)
        {
            var result = await _dispatcher.DispatchAsync<SEOResultsModel>(new GetSEOResultsFromBingSearchQuery(keyword, url));
            return Ok(result);
        }
    }
}
