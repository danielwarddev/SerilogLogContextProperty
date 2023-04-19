using Serilog.Context;
using Serilog;

namespace SerilogLogContextProperty;

public class ServiceWithStaticLogger
{
    public void MethodUsingStaticLogger(bool usePropertyOne)
    {
        var logPropertyName = usePropertyOne ? "propertyOne" : "propertyTwo";
        var logPropertyValue = usePropertyOne ? 1 : 2;

        using (LogContext.PushProperty(logPropertyName, logPropertyValue))
        {
            Log.Logger.Information("Very cool message");
        }
    }
}
