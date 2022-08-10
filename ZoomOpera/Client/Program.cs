using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZoomOpera.Client;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils;
using ZoomOpera.Client.Utils.EventHandlers;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//predefinito
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7288/") });


builder.Services.AddBlazoredLocalStorage();
//config => config.JsonSerializerOptions.WriteIndented = true

//builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<IService<IBuilding, BuildingDTO>, BuildingService>();
builder.Services.AddScoped<IService<ILevel, LevelDTO>, LevelService>();
builder.Services.AddScoped<IService<ILocation, LocationDTO>, LocationService>();
builder.Services.AddScoped<IService<IMonitorPlatform, MonitorPlatformDTO>, MonitorPlatformService>();
builder.Services.AddScoped<IService<IOpera, OperaDTO>, OperaService>();
builder.Services.AddScoped<IService<IOperaImage, OperaImageDTO>, OperaImageService>();
builder.Services.AddScoped<IService<IImageMap, ImageMapDTO>, ImageMapService>();
builder.Services.AddScoped<IService<IImageMapCoordinate, ImageMapCoordinateDTO>, ImageMapCoordinateService>();

builder.Services.AddScoped<IEventHandler<IBuilding>, ModifyBuildingHandler>();
builder.Services.AddScoped<IEventHandler<ILevel>, ModifyLevelsInBuildingHandler>();
builder.Services.AddScoped<IEventHandler<ILocation>, ModifyLocationsInLevelHandler>();
builder.Services.AddScoped<IEventHandler<IMonitorPlatform>, ModifyPlatformsInLevelHandler>();
builder.Services.AddScoped<IEventHandler<IOpera>, ModifyOperaInLocationHandler>();
builder.Services.AddScoped<IEventHandler<IOpera>, AddOperaInLocationHandler>();

builder.Services.AddScoped<ILoginService<IAdmin>, AdminLoginService>(); //TODO: Transient?
builder.Services.AddScoped<ILoginService<IMonitorPlatform>, MonitorPlatformLoginService>();

builder.Services.AddScoped<AuthenticationStateProvider, StateProvider>();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
