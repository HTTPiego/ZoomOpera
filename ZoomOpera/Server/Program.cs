using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ZoomOpera.Server.Data;
using ZoomOpera.Server.Data.Services;
using ZoomOpera.Shared.Data.Services;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

//builder.Services.AddDbContextFactory<ZoomOperaContext>(options =>
builder.Services.AddDbContext<ZoomOperaContext>(options =>
{
    options
    .UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("ZoomOpera"));
});

builder.Services.AddScoped<IService<IBuilding, BuildingDTO>, BuildingService>();
builder.Services.AddScoped<IService<ILevel, LevelDTO>, LevelService>();
builder.Services.AddScoped<IService<ILocation, LocationDTO>, LocationService>();
builder.Services.AddScoped<IService<IOpera, OperaDTO>, OperaService>();
builder.Services.AddScoped<IService<IMonitorPlatform, MonitorPlatformDTO>, MonitorPlatformService>();
builder.Services.AddScoped<IService<IAdmin, AdminDTO>, AdminService>();
builder.Services.AddScoped<IService<IOperaImage, OperaImageDTO>, OperaImageService>();
builder.Services.AddScoped<IService<IImageMap, ImageMapDTO>, ImageMapService>();
builder.Services.AddScoped<IService<IImageMapCoordinate, ImageMapCoordinateDTO>, ImageMapCoordinateService>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.RequireHttpsMetadata = true;
    jwtOptions.SaveToken = true;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
    };
});
builder.Services.AddMvc();
builder.Services.AddControllers();

// per avere le sorta di "ricorsioni" nei json
builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/swagger/v1/swagger.json", "ZoomOpera v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
