namespace MeuMenuPedido.Api.Middlewares;

/// <summary>
/// Middleware para adicionar response body no log do AppInsights
/// </summary>
public class ResponseBodyStoringMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var originalBodyStream = context.Response.Body;

        try
        {
            // Define o corpo da resposta com um stream que permite leitura e execução de seeking
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;
            
            // Executa o próximo middleware da lista e aguarda até a execução finalizar
            await next(context);

            // Lê corpo da resposta a partir do stream
            memoryStream.Position = 0;
            var reader = new StreamReader(memoryStream);
            var responseBody = await reader.ReadToEndAsync();
            
            // Copia o corpo da resposta de volta para o stream original
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);

            context.Items["response_body"] = responseBody;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}

public static class ResponseBodyStoringMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseBodyStoringMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseBodyStoringMiddleware>();
    }
}