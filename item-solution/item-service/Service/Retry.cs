using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace item_service.Service;

public class RetryService
{
    private readonly ILogger<RetryService> _logger;
    private int retryCount = 3;
    private readonly TimeSpan delay = TimeSpan.FromSeconds(5);

    public RetryService(ILogger<RetryService> logger)
    {
        _logger = logger;
    }

    public async Task<T?> RetryFunction<T>(Task<T?> task)
    {
        int currentRetry = 0;
        for (;;)
        {
            try
            {
                // Call external service.
                var result = await task;
                if (result == null) {
                    _logger.LogInformation("result er null");
                    return default;
                }

                // Return or break.
                _logger.LogInformation(nameof(task) + " executed");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"AddCustomer exception: {ex.Message}");

                currentRetry++;
                if (currentRetry >= this.retryCount || !IsTransient(ex))
                {
                    _logger.LogCritical(ex, ex.Message);
                    return default;
                }
                _logger.LogInformation("Trying again");
            }

            // Wait to retry the operation.
            // Consider calculating an exponential delay here and
            // using a strategy best suited for the operation and fault.
            await Task.Delay(delay);
        }
    }

    private bool IsTransient(Exception ex)
    {
        _logger.LogInformation($"Checking if exception {ex.GetType().ToString()} is transient");
        return true;
    }
}
