using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClickShow.Utility
{
    public static class MyJsonConverter
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Deserialize an from json string
        /// </summary>
        public static T Deserialize<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body, Options);
        }

        /// <summary>
        /// Serialize an object to json
        /// </summary>
        public static string Serialize<T>(T item)
        {
            return JsonSerializer.Serialize(item, Options);
        }
    }
}
