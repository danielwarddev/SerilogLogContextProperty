using FluentAssertions;
using Serilog;
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
    public void Log_Context_Should_Have_First_Property_When_True(bool usePropertyOne, string expectedProperty)
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
            logEvent.Properties.Single().Key.Should().Be(expectedProperty);
        }
    }
}