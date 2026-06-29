using Indt.Teste.PropostaService.Api.Middlewares;

namespace Indt.Teste.PropostaService.Api.Extensions;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionHandlingMiddleware>();
}