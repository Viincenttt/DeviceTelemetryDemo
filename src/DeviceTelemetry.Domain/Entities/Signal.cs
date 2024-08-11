using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeviceTelemetry.Domain.Entities;

public record Signal
{
    [JsonConverter(typeof(StringEnumConverter))]
    public required SignalType Type { get; init; }
    
    public required string Value { get; init; }
}