# Distributed.Tracing with Opentelemetry and Openzipkin

![Logo](https://i.postimg.cc/L5LxVjkV/distributed-tracing-icon.png)
https://i.postimg.cc/HnjY8YqZ/Screenshot-2.png
Distributed tracing is a method for tracking requests as they flow through a complex system made up of multiple servers and services. It enables developers to understand the behavior of their distributed systems by providing visibility into the path of a request as it traverses different components of the system.

## How to use:
After adding the library to our project, we have to follow the following steps:

1 - We will add some lines to Program.cs
```javascript

using Distributed.Tracing.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedTracingServies(builder.Configuration);
var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    StartDiagnosticExtension.StartActivity("App-Service1", "StartEndPoint", "Service1 is stated !!");
    endpoints.MapControllers();
});
app.Run();
```
2 - add Elastic URL in appseting.json .
```javascript
{
   "OpenTelemetryConfig": {
    "ListenerName": "Service1",
    "ServiceName": "Service1.API",
    "ServiceVersion": "1.0.0"
  },
  "ZipkinConfig": {
    "ZipkinAddress": "http://localhost:9411/api/v2/spans"
  },
}
```

![Logo](https://i.postimg.cc/zfj8dH52/Screenshot-1.png)

![Logo](https://i.postimg.cc/HnjY8YqZ/Screenshot-2.png)

