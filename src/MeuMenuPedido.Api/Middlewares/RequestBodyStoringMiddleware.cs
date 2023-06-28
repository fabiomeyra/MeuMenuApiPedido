namespace MeuMenuPedido.Api.Middlewares
{
    public class RequestBodyStoringMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyStoringMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            using var streamReader = new StreamReader(httpContext.Request.Body, System.Text.Encoding.UTF8);
            var body = await streamReader.ReadToEndAsync();
            httpContext.Request.Body.Position = 0;

            httpContext.Items["body"] = body;
            await _next(httpContext);
        }
    }

    public static class RequestBodyStoringMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionRequestBodyStringMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestBodyStoringMiddleware>();
        }
    }
}
