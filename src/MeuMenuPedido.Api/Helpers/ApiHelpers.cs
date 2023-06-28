namespace MeuMenuPedido.Api.Helpers
{
    public static class ApiHelpers
    {
        public static string GetRawBodyString(this HttpContext? httpContext) => httpContext == null ? "" : (string)httpContext.Items["body"]!;
        public static string GetRawResponseBodyString(this HttpContext? httpContext) => httpContext == null ? "" : (string)httpContext.Items["response_body"]!;
    }
}