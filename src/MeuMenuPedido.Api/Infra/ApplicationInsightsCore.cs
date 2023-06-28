using MeuMenuPedido.Api.Helpers;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace MeuMenuPedido.Api.Infra
{
    public class ApplicationInsightsCore
    {
        private readonly AppSettings _appSettings;

        public ApplicationInsightsCore(IOptions<AppSettings> opt)
        {
            _appSettings = opt.Value;
        }

        public void AddException(HttpContext context, Exception ex)
        {
            var host = context.Request.Host.Value;
            var endpoint = context.Request.Path.Value;
            var user = context.User.Identity?.Name;
            var body = context.GetRawBodyString();

            // Get Request Telemetry
            var requestTelemetry = context.Features.Get<RequestTelemetry>();
            if (requestTelemetry is null) return;

            using TelemetryConfiguration tConfig = new TelemetryConfiguration { ConnectionString = _appSettings.ApplicationInsights?.ConnectionString };
            var telemetryClient = new TelemetryClient(tConfig);

            var exceptionOperation = new ExceptionTelemetry(ex);

            // Link Exception to Request operation
            exceptionOperation.Context.Operation.Id = requestTelemetry.Context.Operation.Id;
            exceptionOperation.Context.Operation.ParentId = requestTelemetry.Id;
            exceptionOperation.Context.Operation.Name = $"{context.Request.Method} {context.Request.Path.Value}";
            exceptionOperation.Context.User.Id = $"{user}";
            exceptionOperation.Context.Device.Id = Environment.MachineName;
            exceptionOperation.ProblemId = $"{context.Request.Method} {context.Request.Path.Value}";
            exceptionOperation.Message = ex.InnerException?.Message ?? ex.Message;

            requestTelemetry.Properties.Add("Host", host);
            requestTelemetry.Properties.Add("Request", endpoint);
            requestTelemetry.Properties.Add("Body", body);
            requestTelemetry.Properties.Add("User", user);
            requestTelemetry.Properties.Add("Exception Message", RetornaMensagemException(ex));
            requestTelemetry.Properties.Add("Is production", _appSettings.EhAmbienteDeProducao ? "Yes" : "No");

            telemetryClient.TrackException(exceptionOperation);
            telemetryClient.Flush();
        }

        private string RetornaMensagemException(Exception ex)
        {
            if (ex.InnerException is null) return ex.Message;
            return RetornaMensagemException(ex.InnerException);
        }
    }
}