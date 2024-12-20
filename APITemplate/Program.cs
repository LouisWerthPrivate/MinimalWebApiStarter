using APITemplate.Endpoints;
using APITemplate.ServiceExtentions;
using APITemplate.WebApplicationExtentions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddProblemDetails();

builder.Services
    .AddApplicationLogging(builder.Configuration)
    .AddApplicationAuthentication(builder.Configuration)
    .AddApplicationOpenApiDocumentation()
    .AddRepositories()
    .AddApplicationServices()
    .AddApplicationHealthChecks(builder.Configuration);

builder.WebHost.ConfigureKestrel(static options =>
{
    options.AddServerHeader = false;
});

var app = builder.Build();

if (app.Configuration.GetValue<bool>("AdvancedLogging:HttpLogging", defaultValue: false) == true)
{
    app.UseHttpLogging();
}
app.UseAuthentication();
app.UseAuthorization();
app.ConfigureApplication();
app.MapEndpoints();
app.MapApplicationHealthChecks();

app.Run();

