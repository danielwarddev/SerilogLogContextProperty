using FluentAssertions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;

namespace SerilogLogContextProperty.UnitTests;

public class SomeServiceTests
{
    private readonly SomeService _service;

    public SomeServiceTests()
    {
        _service = new SomeService();
    }

    [Theory]
    [InlineData(true, "propertyOne", 1)]
    [InlineData(false, "propertyTwo", 2)]
    public void Log_Context_Should_Have_Correct_Property(bool usePropertyOne, string expectedKey, int expectedValue)
    {
        using (TestCorrelator.CreateContext())
        using (var logger = new LoggerConfiguration()
            .WriteTo.Sink(new TestCorrelatorSink())
            .Enrich.FromLogContext()
            .CreateLogger())
        {
            Log.Logger = logger;

            _service.SomeMethod(usePropertyOne);

            var logEvent = TestCorrelator.GetLogEventsFromCurrentContext().Single();
            var logEventProperty = logEvent.Properties.Single();
            var propertyValue = logEventProperty.Value as ScalarValue;

            logEventProperty.Key.Should().Be(expectedKey);
            propertyValue.Value.Should().Be(expectedValue);
        }
    }
}