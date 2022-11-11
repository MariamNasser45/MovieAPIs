using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MovieAPIs.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//calling type of connection directly without define it in variabl as we made in crud.net project
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // define automapper

builder.Services.AddControllers();

//to access Core from any network
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//defination of swagger to the app can use it .. using options to change from default to need options
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1", //version which i use
        Title = "Frist Swagger API", // title os swagger page ""
        Description = "My first api",
        TermsOfService = new Uri("https://www.google.com"), // action of TermsOfService "put any action link we need"
        Contact = new OpenApiContact
        {
            //initialize information
            Name = "Mariam",
            Email = "SwaggerAPI@domain.com",
            Url = new Uri("https://www.facebook.com")
        },
        License = new OpenApiLicense
        {
            Name = "My license",
            Url = new Uri("https://www.google.com")
        }
    });

    // Security code : to make end user can add token authintecation while send request to APIs
    //1- defination authorize for all end points
    options.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });
    //2- specify token sent with some requestes
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Development only can using this ... if need to exist it on both develepment , production
    //  write them outside condition but not safe for secure

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//important define it beffore authorization
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); // any header , method , origine can access it

app.UseAuthorization();

app.MapControllers();

app.Run();