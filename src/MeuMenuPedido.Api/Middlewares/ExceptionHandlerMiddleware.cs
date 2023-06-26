using System.Net;
using System.Text.Json;

namespace MeuMenuPedido.Api.Middlewares;

/// <summary>
/// Middleware geral de tratamento de erros / exceções
/// </summary>
public class ExceptionHandlerMiddleware
{
    private const string JsonContentType = "application/json";
    private readonly RequestDelegate _request;

    /// <summary>
    /// Inicializa uma nova instância <see cref="ExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next.</param>
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _request = next;
    }

    /// <summary>
    /// Invoca o contexto especificadao async.
    /// </summary>
    /// <param name="httpContext">Contexto</param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _request(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //exception.Ship(context);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = JsonContentType;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                new
                {
                    success = false,
                    errors = new List<string>()
                    {
                        "Ocorreu um erro inesperado no nosso servidor"
                    }

                })
        );
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}