using CompanyService;
using Serilog;
using Serilog.Context;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(path: "serilog.json", optional: false, reloadOnChange: true)
    .Build();

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithThreadId()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddTransient<RequestHandler>();
builder.Services.AddHttpClient<TargetClient>(client => client.BaseAddress = new Uri("https://localhost:7180/"))
    .AddHttpMessageHandler<RequestHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICorrelationIdAccessor, CorrelationIdAccessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<LogHeaderMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (httpContext, next) =>
{
    // Add username to log context, so can be used in template
    var username = httpContext?.User?.Identity?.IsAuthenticated == true ? httpContext.User.Identity.Name : "anonymous";
    LogContext.PushProperty("User", username);

    var ip = httpContext?.Connection?.RemoteIpAddress?.ToString();
    LogContext.PushProperty("IP", !string.IsNullOrEmpty(ip) ? ip : "unknown");

    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();