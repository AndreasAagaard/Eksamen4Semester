using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace auction_service.Service;

public interface IRetryService {
    Task<T?> RetryFunction<T>(Task<T?> task);
    Task<T?> RetryFunctionNoAsync<T>(Func<T?> task);
    Task VoidRetryFunction(Task task);
}
public class RetryService : IRetryService
{
    private readonly ILogger<IRetryService> _logger;
    private int retryCount = 3;
    private readonly TimeSpan delay = TimeSpan.FromSeconds(5);

    public RetryService(ILogger<IRetryService> logger)
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
                _logger.LogInformation(nameof(task) + " executed");

                if (result == null) {
                    _logger.LogInformation("No content");
                    return default;
                }

                // Return or break.
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"AddCustomer exception: {ex.Message}");

                currentRetry++;
                if (currentRetry >= this.retryCount || !IsTransient(ex))
                {
                    _logger.LogCritical(ex, ex.Message);
                    throw;
                }
                _logger.LogInformation("Trying again");
            }

            // Wait to retry the operation.
            // Consider calculating an exponential delay here and
            // using a strategy best suited for the operation and fault.
            await Task.Delay(delay);
        }
    }

    public async Task<T?> RetryFunctionNoAsync<T>(Func<T?> task)
    {
        int currentRetry = 0;
        for (;;)
        {
            try
            {
                // Call external service.
                var result = task();
                _logger.LogInformation(nameof(task) + " executed");

                if (result == null) {
                    _logger.LogInformation("No content");
                    return default;
                }

                // Return or break.
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"AddCustomer exception: {ex.Message}");

                currentRetry++;
                if (currentRetry >= this.retryCount || !IsTransient(ex))
                {
                    _logger.LogCritical(ex, ex.Message);
                    throw;
                }
                _logger.LogInformation("Trying again");
            }

            // Wait to retry the operation.
            // Consider calculating an exponential delay here and
            // using a strategy best suited for the operation and fault.
            await Task.Delay(delay);
        }
    }

    public async Task VoidRetryFunction(Task task)
    {
        int currentRetry = 0;
        for (;;)
        {
            try
            {
                // Call external service.
                await task;
                _logger.LogInformation(nameof(task) + " executed");

                // Return or break.
                return;
            }
            catch (Exception ex)
            {
                _logger.LogTrace($"AddCustomer exception: {ex.Message}");

                currentRetry++;
                if (currentRetry >= this.retryCount || !IsTransient(ex))
                {
                    _logger.LogCritical(ex, ex.Message);
                    throw;
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
