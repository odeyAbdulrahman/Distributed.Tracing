using Distributed.Tracing.Extensions;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributed.Tracing.Middlewares
{
    internal class SetActivteMiddleware
    {
        private readonly RequestDelegate Next;

        public SetActivteMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task Invoke(HttpContext context, Tracer tracer)
        {
            using var parent = tracer.StartActiveSpan($"Middleware Service 2 is started !!");
            await Next(context);
            using var child1 = tracer.StartActiveSpan("Middleware Service 2 is stopped !!");
        }
    }
}
