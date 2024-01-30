using JwtAuthenticationManager;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication();
builder.Services.AddSwaggerForOcelot(builder.Configuration,
(o) =>
{
   // o.GenerateDocsForAggregates = true;
    //o.GenerateDocsForGatewayItSelf = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerForOcelotUI();
await app.UseOcelot();
// .UseOcelot()
// .Wait();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
