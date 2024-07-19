using System.Text.Json;

namespace Core.Utils
{
    public class JsonUtils
    {
        public static string? GetJsonProperties(string jsonString, string fieldName)
        {
            if (string.IsNullOrEmpty(jsonString) || string.IsNullOrEmpty(fieldName)) return null;

            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty(fieldName, out JsonElement element))
                {
                    string? idValue = element.GetString();
                    return string.IsNullOrEmpty(idValue) ? null : idValue;
                }

                string lowerFirstChar = char.ToLower(fieldName[0]) + fieldName.Substring(1);
                if (root.TryGetProperty(lowerFirstChar, out element))
                {
                    string? idValue = element.GetString();
                    return string.IsNullOrEmpty(idValue) ? null : idValue;
                }

                return null;
            }
        }

        public static string[] SplitStringArray(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return Array.Empty<string>();
            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                JsonElement root = doc.RootElement;
                return root.EnumerateArray()
                           .Select(x => x.GetString())
                           .ToArray();
            }
        }
    }
}
