using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeviceTelemetry.Domain.Entities;

public record Telemetry
{
    [JsonProperty(PropertyName = "id")]
    public required Guid CorrelationId { get; init; }
    
    public required Guid DeviceId { get; init; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public required DeviceType DeviceType { get; init; }
    
    public required string EventSource { get; init; }
    
    public required DateTime EventTime { get; init; }

    public required IEnumerable<Signal> Signals { get; init; } = new List<Signal>();
}