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

public class GetSympliSEOResultsFromGoogleSearchQueryHandler(IKeywordSearchServiceFactory keywordSearchServiceFactory) 
    : IQueryHandler<GetSympliSEOResultsFromGoogleSearchQuery, SEOResultsModel>
{
    private readonly IKeywordSearchServiceFactory _keywordSearchServiceFactory = keywordSearchServiceFactory;

    public async Task<SEOResultsModel> HandleAsync(GetSympliSEOResultsFromGoogleSearchQuery query)
    {
        var googleSearchEngine = _keywordSearchServiceFactory.GetService(SearchEngines.Google);
        var results = await googleSearchEngine.GetUrlPositionsResults();
        return new SEOResultsModel(SearchEngines.Google, results);
    }
}
