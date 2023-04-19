using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using Serilog;
using FluentAssertions;

namespace SerilogLogContextProperty.UnitTests;

public class ServiceWithStaticLoggerTests
{
    [Theory]
    [InlineData(true, "propertyOne", 1)]
    [InlineData(false, "propertyTwo", 2)]
    public void Log_Context_Should_Have_Correct_Property_With_Static_Logger(bool usePropertyOne, string expectedKey, int expectedValue)
    {
        using (TestCorrelator.CreateContext())
        using (var logger = new LoggerConfiguration()
            .WriteTo.Sink(new TestCorrelatorSink())
            .Enrich.FromLogContext()
            .CreateLogger())
        {
            Log.Logger = logger;

            var service = new ServiceWithStaticLogger();
            service.MethodUsingStaticLogger(usePropertyOne);

            var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().Single();
            var logEventProperty = logEvent.Properties.Single();
            var propertyValue = logEventProperty.Value as ScalarValue;

            logEventProperty.Key.Should().Be(expectedKey);
            propertyValue!.Value.Should().Be(expectedValue);
        }
    }
}
