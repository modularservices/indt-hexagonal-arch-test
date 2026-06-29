using Indt.Teste.ContratacaoService.Api.Extensions;
using Indt.Teste.ContratacaoService.Application.Ports.In;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using Indt.Teste.ContratacaoService.Application.UseCases;
using Indt.Teste.ContratacaoService.Infra.Integrations;
using Indt.Teste.ContratacaoService.Infra.Messaging;
using Indt.Teste.ContratacaoService.Infra.Messaging.Consumers;
using Indt.Teste.ContratacaoService.Infra.Persistence;
using Indt.Teste.ContratacaoService.Infra.Persistence.Dapper;
using Serilog;
using Serilog.Events;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, cfg) => cfg
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "ContratacaoService")
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture,
                         outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{CorrelationId}] {SourceContext} {Message:lj}{NewLine}{Exception}"));

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IListContratacoesUseCase, ListContratacoesUseCase>();
builder.Services.AddScoped<IGetContratacaoByIdUseCase, GetContratacaoByIdUseCase>();
builder.Services.AddScoped<IContratacaoRepository, ContratacaoRepository>();

builder.Services.AddHttpClient<IPropostaServiceClient,
                                PropostaServiceClient>
                                ((sp, client) =>
                                {
                                    var configuration = sp.GetRequiredService<IConfiguration>();
                                    var baseUrl = configuration["Services:PropostaService:BaseUrl"];
                                    client.BaseAddress = new Uri(baseUrl!);
                                });

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddHostedService<PropostaAprovadaConsumer>();

var app = builder.Build();

app.MapHealthChecks("/health");

// if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.UseGlobalExceptionHandler();
app.UseCorrelationId();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
