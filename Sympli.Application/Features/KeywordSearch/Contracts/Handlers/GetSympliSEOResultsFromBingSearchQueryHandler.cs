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

public class GetSympliSEOResultsFromBingSearchQueryHandler(IKeywordSearchServiceFactory keywordSearchServiceFactory) 
    : IQueryHandler<GetSympliSEOResultsFromBingSearchQuery, SEOResultsModel>
{
    private readonly IKeywordSearchServiceFactory _keywordSearchServiceFactory = keywordSearchServiceFactory;

    public async Task<SEOResultsModel> HandleAsync(GetSympliSEOResultsFromBingSearchQuery query)
    {
        var bingSearchEngine = _keywordSearchServiceFactory.GetService(SearchEngines.Bing);
        var results = await bingSearchEngine.GetUrlPositionsResults();
        return new SEOResultsModel(SearchEngines.Bing, results);
    }
}
