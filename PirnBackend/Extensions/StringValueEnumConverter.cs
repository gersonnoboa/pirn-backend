namespace PirnBackend.Extensions;

using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public class StringValueEnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumType = typeof(T);
        foreach (var field in enumType.GetFields())
        {
            var attribute = field.GetCustomAttribute<StringValueAttribute>();
            if (attribute != null && attribute.Value == reader.GetString())
            {
                return (T)Enum.Parse(enumType, field.Name);
            }
        }
        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<StringValueAttribute>();
        writer.WriteStringValue(attribute?.Value ?? value.ToString());
    }
}
