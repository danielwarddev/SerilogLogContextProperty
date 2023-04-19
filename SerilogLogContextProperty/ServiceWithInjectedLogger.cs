using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace SerilogLogContextProperty;

public class ServiceWithInjectedLogger
{
    private readonly ILogger<ServiceWithInjectedLogger> _logger;

    public ServiceWithInjectedLogger(ILogger<ServiceWithInjectedLogger> logger)
    {
        _logger = logger;
    }

    public void MethodUsingInjectedLogger(bool usePropertyOne)
    {
        var logPropertyName = usePropertyOne ? "propertyOne" : "propertyTwo";
        var logPropertyValue = usePropertyOne ? 1 : 2;

        using (LogContext.PushProperty(logPropertyName, logPropertyValue))
        {
            _logger.LogInformation("Very cool message");
        }
    }
}
