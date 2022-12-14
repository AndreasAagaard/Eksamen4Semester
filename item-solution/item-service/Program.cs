using item_service.Service;
using NLog; 
using NLog.Web; 
using Microsoft.Net.Http.Headers;

var logger = 
NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger(); 
logger.Debug("init main");
try{
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CatalogService>();
builder.Services.AddTransient<MongoDBContext>();
builder.Services.AddTransient<RetryService>();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("gateway", client =>
{
   client.BaseAddress = new Uri("http://gateway:4000/");
   client.DefaultRequestHeaders.Add(
      HeaderNames.Accept, "application/json");
}
);
builder.Logging.ClearProviders(); 
builder.Host.UseNLog(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.Run();
}
catch (Exception ex) 
{ 
   logger.Error(ex, "Stopped program because of exception"); 
   throw; 
} 
finally 
{ 
   NLog.LogManager.Shutdown(); 
} 