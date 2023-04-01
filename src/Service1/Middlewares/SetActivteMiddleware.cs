using Distributed.Tracing.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            using var parent = tracer.StartActiveSpan("Middleware Service 1 is started !!");
            await Next(context);
            using var child1 = tracer.StartActiveSpan("Middleware Service 1 is stopped !!");
        }
    }
}
