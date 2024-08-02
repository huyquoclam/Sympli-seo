using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sympli.WebAPI.Filters;

public class ConcurrentRateLimiterFilterAttribute: ActionFilterAttribute
{
    private readonly object _lock = new();

    public ConcurrentRateLimiterFilterAttribute()
    {
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        int limit = 1;

        lock (_lock)
        {
            if (ConcurrencyRateLimiterCounter.Instance.RunningCounter >= limit)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);
                return;
            }

            ConcurrencyRateLimiterCounter.Instance.RunningCounter++;
        }

        await next();
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        lock (_lock)
        {
            if(ConcurrencyRateLimiterCounter.Instance.RunningCounter > 0)
                ConcurrencyRateLimiterCounter.Instance.RunningCounter--;
        }

        await next();
    }

    class ConcurrencyRateLimiterCounter
    {
        public int RunningCounter = 0;
        static ConcurrencyRateLimiterCounter _instance = new();
        public static ConcurrencyRateLimiterCounter Instance => _instance;
        private ConcurrencyRateLimiterCounter() { }
    }
}
