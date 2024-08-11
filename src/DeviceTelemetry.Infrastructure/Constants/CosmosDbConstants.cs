namespace DeviceTelemetry.Infrastructure.Constants;

public static class CosmosDbConstants
{
    public static class DatabaseNames
    {
        public const string DeviceTelemetry = "devicetelemetry";
    }

    public static class ContainerNames
    {
        public const string Telemetry = "telemetry";
        public const string TelemetryLease = "telemetrylease";
    }
}