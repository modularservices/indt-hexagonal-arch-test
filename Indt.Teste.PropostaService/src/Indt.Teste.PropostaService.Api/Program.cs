using Indt.Teste.PropostaService.Api.Extensions;
using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;
using Indt.Teste.PropostaService.Application.UseCases;
using Indt.Teste.PropostaService.Infra.Messaging;
using Indt.Teste.PropostaService.Infra.Persistence;
using Indt.Teste.PropostaService.Infra.Persistence.Dapper;
using Indt.Teste.PropostaService.Infra.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, cfg) => cfg
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "PropostaService")
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture,
                         outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {SourceContext} {Message:lj}{NewLine}{Exception}"));

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// EF Core + SQL Server
builder.Services.AddDbContext<PropostaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IPropostaRepository, PropostaRepository>();
builder.Services.AddScoped<ICreatePropostaUseCase, CreatePropostaUseCase>();
builder.Services.AddScoped<IGetPropostaByIdUseCase, GetPropostaByIdUseCase>();
builder.Services.AddScoped<IListPropostasUseCase, ListPropostasUseCase>();
builder.Services.AddScoped<IUpdatePropostaStatusUseCase, UpdatePropostaStatusUseCase>();
builder.Services.AddScoped<IContractPropostaUseCase, ContractPropostaUseCase>();

var app = builder.Build();

app.MapHealthChecks("/health");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.UseGlobalExceptionHandler();
app.UseCorrelationId();
app.UseSerilogRequestLogging();

// Controllers
app.MapControllers();

if (!app.Environment.IsEnvironment("Testing"))
    await app.SetupDatabaseAsync();

app.Run();

public partial class Program
{
}