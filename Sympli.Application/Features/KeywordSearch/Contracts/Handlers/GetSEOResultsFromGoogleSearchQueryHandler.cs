using Sympli.Application.Common;
using Sympli.Application.CQRS.Messaging;
using Sympli.Application.Features.KeywordSearch.Contracts.Queries;
using Sympli.Application.Features.KeywordSearch.Contracts.Views;
using Sympli.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Features.KeywordSearch.Contracts.Handlers;

public class GetSEOResultsFromGoogleSearchQueryHandler(IKeywordSearchServiceFactory keywordSearchServiceFactory) 
    : IQueryHandler<GetSEOResultsFromGoogleSearchQuery, SEOResultsModel>
{
    private readonly IKeywordSearchServiceFactory _keywordSearchServiceFactory = keywordSearchServiceFactory;

    public async Task<SEOResultsModel> HandleAsync(GetSEOResultsFromGoogleSearchQuery query)
    {
        query.Validate();
        query.Sanitize();

        var googleSearchEngine = _keywordSearchServiceFactory.GetService(SearchEngines.Google);
        var results = await googleSearchEngine.GetUrlPositionsResultsWithCustomKeywordsAndUrl(query.Keyword, query.Url);
        return new SEOResultsModel(SearchEngines.Google, results);
    }
}
