using Sympli.Application.Common;
using Sympli.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Features.KeywordSearch.Contracts.Queries;

public class BaseGetSEOResultsFromEngineSearchQuery(string keyword, string url) : IQuery
{
    public string Keyword { get; set; } = keyword;
    public string Url { get; set; } = url;

    public void Validate()
    {
        Assertion.StringNotNullOrEmpty(Keyword, nameof(Keyword));
        Assertion.UrlIsValid(Url, nameof(Url));
    }

    public void Sanitize()
    {
        Keyword = Keyword.Trim();
        // remove http:// or https:// from url
        Url = Url.Trim().Replace("http://www.", "").Replace("https://www.", "");
    }
}
