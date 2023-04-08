using Serilog;
using Serilog.Context;

namespace SerilogLogContextProperty;

public class SomeService
{
    public void SomeMethod(bool usePropertyOne)
    {
        var logPropertyName = usePropertyOne ? "propertyOne" : "propertyTwo";
        var logPropertyValue = usePropertyOne ? 1 : 2;

        using (LogContext.PushProperty(logPropertyName, logPropertyValue))
        {
            Log.Logger.Information("Very cool message");
        }
    }
}
