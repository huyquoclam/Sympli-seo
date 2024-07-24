using Sympli.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Features.KeywordSearch.Contracts.Views;

public class SEOResultsModel(string searchEngine, List<int> positions) : IView
{
    public string SearchEngine { get; set; } = searchEngine;
    public List<int> Positions { get; set; } = positions;
}
