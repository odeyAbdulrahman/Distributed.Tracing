using Distributed.Tracing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

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
builder.Services.AddDistributedTracingServies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    StartDiagnosticExtension.StartActivity("App-Service1", "StartEndPoint", "Service1 is stated !!");
    endpoints.MapControllers();
});
app.Run();