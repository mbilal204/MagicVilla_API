using MagicVilla_VillaAPI.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
//    .WriteTo.File("log/villaLogs", rollingInterval: RollingInterval.Day).CreateLogger();

//builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers(option => { 
   // option.ReturnHttpNotAcceptable = true; 
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
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
