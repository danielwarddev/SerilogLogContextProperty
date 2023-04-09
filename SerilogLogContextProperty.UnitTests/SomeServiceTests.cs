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
    [InlineData(true, "propertyOne")]
    [InlineData(false, "propertyTwo")]
    public void Log_Context_Should_Have_Correct_Property(bool usePropertyOne, string expectedKey)
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
            logEventProperty.Key.Should().Be(expectedKey);
            logEventProperty.Value.Should().Be(1);
        }
    }
}