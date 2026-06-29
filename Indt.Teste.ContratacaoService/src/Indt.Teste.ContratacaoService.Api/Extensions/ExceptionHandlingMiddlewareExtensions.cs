using Indt.Teste.ContratacaoService.Api.Middlewares;

namespace Indt.Teste.ContratacaoService.Api.Extensions;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionHandlingMiddleware>();
}