﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Utils;

namespace Executes.Models.JsonConverters
{
    public class QAngleJsonConverter : JsonConverter<QAngle>
    {
        public override QAngle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string value.");
            }

            var stringValue = reader.GetString();
            if (stringValue == null)
            {
                throw new JsonException("String value is null.");
            }

            var values = stringValue.Split(' '); // Split by space

            Console.WriteLine($"[Executes] QAngle values: {stringValue}");

            if (values.Length != 3)
            {
                throw new JsonException($"String value '{stringValue}' is not in the correct format (X Y Z).");
            }

            if (!float.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x) ||
                !float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y) ||
                !float.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var z))
            {
                Console.WriteLine($"[Executes] Unable to parse QAngle float values for: '{stringValue}'");
                throw new JsonException($"Unable to parse QAngle float values for '{stringValue}'.");
            }

            return new QAngle(x, y, z);
        }

        public override void Write(Utf8JsonWriter writer, QAngle value, JsonSerializerOptions options)
        {
            // Convert Vector object to string representation (example assumes ToString() returns desired format)
            var qangleString = value.ToString();
            writer.WriteStringValue(qangleString);
        }
    }
}