using System.Net;
using MeuMenuPedido.Api.Helpers;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace MeuMenuPedido.Api.Infra;

public class TelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static AppSettings? _appSettings;

    public TelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public void Initialize(ITelemetry telemetry)
    {
        //  para statusCode 500 já salvamos o request com a exceção no ApplicationInsightsCore
        if (_httpContextAccessor.HttpContext?.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            return;

        var config = _httpContextAccessor.HttpContext?.RequestServices?.GetRequiredService<IOptions<AppSettings>>().Value;

        _appSettings ??= config;

        var requestTelemetry = telemetry as RequestTelemetry;
        if (requestTelemetry == null) return;

        var host = _httpContextAccessor.HttpContext?.Request.Host.Value;
        var endpoint = _httpContextAccessor.HttpContext?.Request.Path.Value;
        var user = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        var body = _httpContextAccessor.HttpContext?.GetRawBodyString();
        var responseBody = _httpContextAccessor.HttpContext?.GetRawResponseBodyString();

        requestTelemetry.Context.User.Id = $"{user}";
        requestTelemetry.Context.Device.Id = Environment.MachineName;

        requestTelemetry.Properties.Add("Host", host);
        requestTelemetry.Properties.Add("Request", endpoint);
        requestTelemetry.Properties.Add("Body", body);
        requestTelemetry.Properties.Add("User", user);
        requestTelemetry.Properties.Add("Is production?", _appSettings?.EhAmbienteDeProducao ?? false ? "Yes" : "No");
        
        // Adiciona o ResponseBody na telemetria do application insights
        requestTelemetry.Properties.Add("ResponseBody", responseBody);
    }
}