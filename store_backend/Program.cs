using Microsoft.OpenApi.Models;
using store_backend.Authorization;
using store_backend.Context;
using store_backend.Dao;
using store_backend.Helpers;
using store_backend.Middlewares;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//! To enable insert DateTime 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Store API",
        Description = "An ASP.NET Core Web API for managing store items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Carlos Daniel Rivera",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "CodeWave",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
//for add the initial objects
builder.Services.AddNpgsql<ApplicationContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    //Serialize enums as string in api responses (for roles)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//Auth
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
//All the different Daos
builder.Services.AddScoped<IProductDao, ProductDao>();
builder.Services.AddScoped<IPersonaDao, PersonaDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
//Middlewares
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
