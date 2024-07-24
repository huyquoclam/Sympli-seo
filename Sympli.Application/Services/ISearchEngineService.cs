using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Services;

public interface ISearchEngineService
{
    /// <summary>
    /// Get Sympli SEO results from the engine search with default keywords and url
    /// </summary>
    /// <returns></returns>
    Task<List<int>> GetUrlPositionsResults();

    /// <summary>
    /// Get Sympli SEO results from the engine search with custom keywords and url
    /// </summary>
    /// <param name="keywords"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    Task<List<int>> GetUrlPositionsResultsWithCustomKeywordsAndUrl(string keywords, string url);

}
