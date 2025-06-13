using System.Text.Json;
using System.Text.Json.Serialization;

namespace Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils
{
    public class JsonDateConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
                return date;
            }
            throw new JsonException($"Invalid date format. Expected {DateFormat}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat));
        }
    }
  
}
