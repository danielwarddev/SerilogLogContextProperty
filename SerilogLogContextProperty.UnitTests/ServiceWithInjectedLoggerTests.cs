using FluentAssertions;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.TestCorrelator;

namespace SerilogLogContextProperty.UnitTests;

public class ServiceWithInjectedLoggerTests
{
    [Theory]
    [InlineData(true, "propertyOne", 1)]
    [InlineData(false, "propertyTwo", 2)]
    public void Log_Context_Should_Have_Correct_Property_With_Injected_Logger(bool usePropertyOne, string expectedKey, int expectedValue)
    {
        using (TestCorrelator.CreateContext())
        using (var serilogLogger = new LoggerConfiguration()
            .WriteTo.Sink(new TestCorrelatorSink())
            .Enrich.FromLogContext()
            .CreateLogger())
        {
            var microsoftLogger = new SerilogLoggerFactory(serilogLogger).CreateLogger<ServiceWithInjectedLogger>();
            var service = new ServiceWithInjectedLogger(microsoftLogger);

            service.MethodUsingInjectedLogger(usePropertyOne);

            var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().Single();
            var logEventProperty = logEvent.Properties.Where(x => x.Key == expectedKey).Single();
            var propertyValue = logEventProperty.Value as ScalarValue;

            logEventProperty.Key.Should().Be(expectedKey);
            propertyValue!.Value.Should().Be(expectedValue);
        }
    }
}