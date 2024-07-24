using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Services;

public interface IKeywordSearchServiceFactory
{
    ISearchEngineService GetService(string serviceName);
}
