# Mako-IoT.Device.Utilities.Invoker
Gracefully retries given call when exception occurs. This is common technique for [transient fault handling](https://learn.microsoft.com/en-us/azure/architecture/best-practices/transient-faults).

## Usage
```c#
HttpClient httpClient = new();
string apiResult;

//try calling REST API max. three times
Invoker.Retry(() =>
{
    using var response = httpClient.Get(apiUrl);
    response.EnsureSuccessStatusCode();
    apiResult = response.Content.ReadAsString();
}, 3, (ex, attempt) =>
{
    _logger.LogError("HttpClient.Get exception", ex);
    return true;
});
```
