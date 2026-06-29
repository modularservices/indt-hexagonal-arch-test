using Indt.Teste.PropostaService.Api.Middlewares;

namespace Indt.Teste.PropostaService.Api.Extensions;

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app) =>
        app.UseMiddleware<CorrelationIdMiddleware>();
}