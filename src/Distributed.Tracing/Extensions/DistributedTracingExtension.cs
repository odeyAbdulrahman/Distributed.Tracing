
using Distributed.Tracing.Services;
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
        var ZipkinUrl = configuration.GetSection("ZipkinConfig")["ZipkinAddress"];
        var OpenTelemetry = configuration.GetSection("OpenTelemetryConfig").Get<OpenTelemetryViewModel>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"localhost:6379,password=12345,ssl=false,abortConnect=false";
        });
        services.AddSingleton(TracerProvider.Default.GetTracer(OpenTelemetry.ServiceName));
        services.AddScoped<ICounter, CounterService>();

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
            .AddHttpClientInstrumentation(options =>
                options.Enrich = (Activity, eventName, rowObject) =>
                {
                    if (eventName == "OnStartActivity" && rowObject is HttpRequestMessage request)
                    {
                        if (request.Method == HttpMethod.Get)
                            Activity.SetTag("GetRequest", "get-request");
                        if (request.Method == HttpMethod.Post)
                            Activity.SetTag("PostRequest", "post-request");
                        if (request.Method == HttpMethod.Put)
                            Activity.SetTag("PutRequest", "put-request");
                        if (request.Method == HttpMethod.Delete)
                            Activity.SetTag("DeleteRequest", "delete-request");
                    }
                })
            .AddAspNetCoreInstrumentation(options =>
            {
                options.Filter = httpContext =>
                    !httpContext.Request.Path.Value.StartsWith("/swagger") &&
                    !httpContext.Request.Path.Value.StartsWith("/_framework") &&
                    !httpContext.Request.Path.Value.StartsWith("/_search") &&
                    !httpContext.Request.Path.Value.StartsWith("/_vs");
            }
             )
            .AddConsoleExporter()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri(ZipkinUrl);
            });
        });
    }
}