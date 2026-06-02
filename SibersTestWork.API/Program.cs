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

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

builder.Services.AddDataBase(connectionString);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddServices();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandler>();
app.MapControllers();
app.Run();