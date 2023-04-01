using Distributed.Tracing.Services;
using OpenTelemetry.Trace;

namespace Distributed.Tracing.Middlewares
{
    internal class SetActivteMiddleware
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly RequestDelegate Next;

        public SetActivteMiddleware(RequestDelegate next, IServiceProvider serviceProvider )
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

            using var parent = tracer.StartActiveSpan("Middleware Service 1 is started !!");
            parent.SetAttribute("count.value", count);

            await Next(context);
            using var stopped = tracer.StartActiveSpan("Middleware Service 1 is stopped !!");
        }

        
    }
}
