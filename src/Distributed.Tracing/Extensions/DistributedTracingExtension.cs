
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Distributed.Tracing.Extensions;

public static class DistributedTracingExtension
{
    public static void AddDistributedTracingServies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetryViewModel>(configuration.GetSection("OpenTelemetryConfig"));

        var OpenTelemetry = configuration.GetSection("OpenTelemetryConfig").Get<OpenTelemetryViewModel>();
        var ZipkinUrl = configuration.GetSection("ZipkinConfig")["ZipkinAddress"];
        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            _ = tracerProviderBuilder
            .AddOtlpExporter(opt =>
             {
                 opt.Protocol = OtlpExportProtocol.HttpProtobuf;
             })
            .AddSource(OpenTelemetry.ServiceName)
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(serviceName: OpenTelemetry.ServiceName, serviceVersion: OpenTelemetry.ServiceVersion))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri(ZipkinUrl);
            });
        });
    }
}