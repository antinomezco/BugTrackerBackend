using BugTracker;
using BugTracker.Repositories;
using BugTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.

var services = builder.Services;

services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.
           Add(new JsonStringEnumConverter());

    options.JsonSerializerOptions.DefaultIgnoreCondition =
             JsonIgnoreCondition.WhenWritingNull;

    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();
    
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey=true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"])),
        ClockSkew = TimeSpan.FromSeconds(5)
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bug Tracker", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});
services.AddAutoMapper(typeof(Program));
services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsUser", pol => pol.RequireClaim("isUser"));
    opt.AddPolicy("IsDeveloper", pol => pol.RequireClaim("isDeveloper"));
    opt.AddPolicy("IsProjectManager", pol => pol.RequireClaim("isProjectManager"));
    opt.AddPolicy("IsAdmin", pol => pol.RequireClaim("isAdmin"));
});

services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<ITicketService, TicketService>();
services.AddScoped<IPersonService, PersonService>();
services.AddScoped<ITicketRepository, TicketRepository>();
services.AddScoped<IProjectRepository, ProjectRepository>();
services.AddScoped<IPersonRepository, PersonRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
