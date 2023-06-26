using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeuMenuPedido.Api.Helpers;


public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _formatos;
    public CustomDateTimeConverter(string[] formatos)
    {
        _formatos = formatos;
    }

    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        foreach (var formato in _formatos)
        {
            try
            {
                if (date is { Second: 0, Minute: 0, Hour: 0 } && formato.Contains("ss")) continue;

                if (date is { Minute: 0, Hour: 0 } && formato.Contains("mm")) continue;

                if (date is { Hour: 0, Minute: 0, Second: 0 } && formato.Contains("HH")) continue;

                writer.WriteStringValue(date.ToString(formato));
                return;
            }
            catch
            {
                // Tenta aplicar o próximo formato na lista
            }
        }
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        foreach (var formato in _formatos)
        {
            try
            {
                var retorno = DateTime.ParseExact(reader.GetString() ?? string.Empty, formato, CultureInfo.InvariantCulture);
                return retorno;
            }
            catch
            {
                // Tenta aplicar o próximo formato na lista
            }
        }

        return DateTime.MinValue;
    }
}