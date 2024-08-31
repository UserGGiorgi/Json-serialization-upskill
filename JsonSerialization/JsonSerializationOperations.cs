using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: CLSCompliant(true)]

namespace JsonSerialization;

public static class JsonSerializationOperations
{
    public static string SerializeObjectToJson(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return JsonSerializer.Serialize(obj);
    }

    public static T? DeserializeJsonToObject<T>(string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        return JsonSerializer.Deserialize<T>(json);
    }

    public static string SerializeCompanyObjectToJson(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var options = new JsonSerializerOptions
        {
            WriteIndented = false, // Ensure compact JSON without newlines
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        return JsonSerializer.Serialize(obj, options);
    }

    public static T? DeserializeCompanyJsonToObject<T>(string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        return JsonSerializer.Deserialize<T>(json);
    }

    public static string SerializeDictionary(Company obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            WriteIndented = false,
        };

        var domainsDict = obj.Domains;

        if (domainsDict == null)
        {
            return "{}";
        }

        var camelCaseDict = domainsDict.ToDictionary(
            kvp => ToCamelCase(kvp.Key),
            kvp => kvp.Value);

        return JsonSerializer.Serialize(camelCaseDict, options);
    }

    public static string SerializeEnum(Company obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        };

        return JsonSerializer.Serialize(obj, options);
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value) || !char.IsUpper(value[0]))
        {
            return value;
        }

        return char.ToLower(value[0], CultureInfo.InvariantCulture) + value[1..];
    }
}
