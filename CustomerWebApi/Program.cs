using CustomerWebApi;
using CustomerWebApi.Services;
using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

// http client
builder.Services.AddHttpClient();

/* Database Context Dependency Injection */
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source=customer-db;Initial Catalog=dms_customer;User ID=sa;Password=password@12345#";
//var connectionString = "Server=localhost,18001;Initial Catalog=dms_customer;User ID=sa;Password=password@12345#";
builder.Services.AddDbContext<CustomerDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer api", Version = "v1.0.0" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddGrpc();
/* ===================================== */

var app = builder.Build();
// Configure the HTTP request pipeline.
// swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoint =>
    {
        endpoint.MapControllers();
        endpoint.MapGrpcService<GreeterService>();
    }
);
//app.UseHttpsRedirection();
app.Run();
