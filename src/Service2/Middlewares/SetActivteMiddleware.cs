using Distributed.Tracing.Services;
using OpenTelemetry.Trace;

namespace Distributed.Tracing.Middlewares
{
    internal class SetActivteMiddleware
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly RequestDelegate Next;

        public SetActivteMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            Next = next;
            ServiceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context, Tracer tracer)
        {
            using var scope = ServiceProvider.CreateScope();
            var scopedCounter = scope.ServiceProvider.GetRequiredService<ICounter>();

            var count = await scopedCounter.GetCounter();
            if (count > 3)
                count = await scopedCounter.SetCounter();

            using var started = tracer.StartActiveSpan($"Middleware Service 2 is started !!");
            started.SetAttribute("count.value", count);
            await Next(context);
            using var stopped = tracer.StartActiveSpan("Middleware Service 2 is stopped !!");
        }
    }
}
