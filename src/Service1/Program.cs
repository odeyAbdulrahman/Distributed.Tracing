using Distributed.Tracing;
using Distributed.Tracing.Extensions;
using Distributed.Tracing.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("Service2", client =>
{
    client.BaseAddress = new Uri("https://localhost:7133"); //api url
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

var OpenTelemetry = builder.Configuration.GetSection("OpenTelemetryConfig").Get<OpenTelemetryViewModel>();

builder.Services.AddDistributedTracingServies(builder.Configuration);
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(OpenTelemetry.ServiceName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<SetActivteMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();