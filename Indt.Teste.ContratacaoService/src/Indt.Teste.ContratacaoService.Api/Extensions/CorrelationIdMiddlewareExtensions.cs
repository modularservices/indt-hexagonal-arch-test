using Indt.Teste.ContratacaoService.Api.Middlewares;

namespace Indt.Teste.ContratacaoService.Api.Extensions;

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app) =>
        app.UseMiddleware<CorrelationIdMiddleware>();
}