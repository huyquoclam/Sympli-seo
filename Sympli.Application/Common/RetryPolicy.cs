using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sympli.Application.Common;

public class RetryPolicy
{
    public static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, int maxRetryAttempts, TimeSpan delay)
    {
        int retryAttempts = 0;

        while (retryAttempts < maxRetryAttempts)
        {
            try
            {
                return await action();
            }
            catch (Exception ex) when (retryAttempts < maxRetryAttempts)
            {
                retryAttempts++;
                Console.WriteLine($"Attempt {retryAttempts} failed: {ex.Message}");

                if (retryAttempts >= maxRetryAttempts)
                {
                    throw; // Re-throw the exception if maximum retry attempts are reached
                }

                // Wait before retrying
                await Task.Delay(delay);
            }
        }

        throw new InvalidOperationException("Reached maximum retry attempts without success.");
    }

}