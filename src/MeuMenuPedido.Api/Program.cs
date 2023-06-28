using System.Globalization;
using System.Text.Json.Serialization;
using MeuMenuPedido.Api.Helpers;
using MeuMenuPedido.Api.Infra;
using MeuMenuPedido.Api.Middlewares;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using MeuMenuPedido.Infra.IoC;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using AuthorizationMiddleware = MeuMenuPedido.Api.Middlewares.AuthorizationMiddleware;

var builder = WebApplication.CreateBuilder(args);

// config AppSettings a ser usado
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{env}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.AllowTrailingCommas = true;

    // configura a API para receber e retornar datas no padrão BR
    x.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter(new[]
        { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "dd/MM/yyyy"  }));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.ResolveDependencias();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var configuracao = appSettingsSection.Get<AppSettings>();

// add Applicaiton Insights
if (configuracao?.ApplicationInsights?.ConnectionString != null)
{
    builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
    {
        ConnectionString = configuracao.ApplicationInsights.ConnectionString
    });

    builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, _) => { module.EnableSqlCommandTextInstrumentation = true; });
}

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddleware>();
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
builder.Services.AddSingleton<ApplicationInsightsCore>();
builder.Services.AddSingleton<ResponseBodyStoringMiddleware>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuracao?.Jwt?.Issuer,
        ValidateAudience = true,
        ValidAudience = configuracao?.Jwt?.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = configuracao?.Jwt?.RetornaKey(),
        ValidateLifetime = true,
        ClockSkew = configuracao!.Jwt!.ExpiresSpan
    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Desenvolvimento",
        b =>
            b
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


    options.AddPolicy("Producao",
        b =>
            b
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithOrigins("")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!(configuracao?.EhAmbienteDeProducao ?? false))
{
    app.UseCors("Desenvolvimento");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseCors("Producao");
    app.UseHsts();
}

app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Meu Menu Pedido V1");
});
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

// Adicionando Middleware para tratar exceções
app.UseExceptionHandlerMiddleware();

app.UseExceptionRequestBodyStringMiddleware();

app.UseResponseBodyStoringMiddleware();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    SupportedCultures = new List<CultureInfo>
    {
        new("pt-BR")
    }
});

// redirecionamento para página inicial do swagger
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.MapControllers();

app.Run();
