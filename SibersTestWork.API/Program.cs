using Serilog;
using SibersDataManager;
using SibersServices;
using SibersTestWork.MiddleWare;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

Log.Logger = new LoggerConfiguration() 
    .ReadFrom.Configuration(builder.Configuration) 
    
    .Enrich.FromLogContext() 
    
    .WriteTo.Console() 
    
    .CreateLogger(); 

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDataBase(connectionString);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddServices();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}
app.UseStaticFiles();

app.UseMiddleware<ExceptionHandler>();
app.MapControllers();
app.Run();