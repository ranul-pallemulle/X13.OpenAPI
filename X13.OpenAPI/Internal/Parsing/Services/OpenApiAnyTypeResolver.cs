using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;
using X13.OpenAPI.Public.Model.Types.Primitives;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal static class OpenApiAnyTypeResolver
    {
        public static IOpenApiAny Resolve(IOpenApiAny any, OpenApiSchema schema = null)
        {
            if (any is OpenApiArray arrayAny)
            {
                var newArray = new OpenApiArray();
                foreach (var element in arrayAny)
                {
                    newArray.Add(Resolve(element, schema?.Items));
                }
                return newArray;
            }
            if (any is OpenApiObject objectAny)
            {
                var newObject = new OpenApiObject();
                foreach (var propertyName in objectAny.Keys.ToList())
                {
                    if (schema?.Properties != null && schema.Properties.ContainsKey(propertyName))
                    {
                        newObject[propertyName] = Resolve(objectAny[propertyName], schema.Properties[propertyName]);
                    }
                    else if (schema?.AdditionalProperties != null && schema.AdditionalProperties is OpenApiSchema ss)
                    {
                        newObject[propertyName] = Resolve(objectAny[propertyName], ss);
                    }
                }
                return newObject;
            }

            if (!(any is OpenApiString))
            {
                return any; // already a non-string OpenApiAny
            }
            var value = ((OpenApiString)any).Value;
            if (((OpenApiString)any).Quoted) // quoted string
            {
                if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                {
                    return new OpenApiDateTime(dateTimeValue);
                }

                return any; // actual type is OpenApiString
            }
            if (value == null)
            {
                return new OpenApiNull();
            }
            if (schema?.Type == null)
            {
                if (value == "true")
                {
                    return new OpenApiBoolean(true);
                }
                if (value == "false")
                {
                    return new OpenApiBoolean(false);
                }
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    return new OpenApiInt32(intValue);
                }

                if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    return new OpenApiInt64(longValue);
                }

                if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    return new OpenApiDouble(doubleValue);
                }

                if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                {
                    return new OpenApiDateTime(dateTimeValue);
                }
            }
            else
            {
                if (schema.Type == "integer")
                {
                    if (schema.Format != null && schema.Format == "int64")
                    {
                        if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                        {
                            return new OpenApiInt64(longValue);
                        }
                    }
                    else
                    {
                        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                        {
                            return new OpenApiInt32(intValue);
                        }
                    }
                }
                if (schema.Type == "number")
                {
                    if (schema.Format != null && schema.Format == "float")
                    {
                        if (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var floatValue))
                        {
                            return new OpenApiFloat(floatValue);
                        }
                    }
                    else
                    {
                        if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                        {
                            return new OpenApiDouble(doubleValue);
                        }
                    }
                }
                if (schema.Type == "string")
                {
                    if (schema.Format != null && schema.Format == "byte")
                    {
                        try
                        {
                            return new OpenApiByte(Convert.FromBase64String(value));
                        }
                        catch (FormatException)
                        { }
                    }
                    else if (schema.Format != null && schema.Format == "binary")
                    {
                        try
                        {
                            return new OpenApiBinary(Encoding.UTF8.GetBytes(value));
                        }
                        catch (EncoderFallbackException)
                        { }
                    }
                    else if (schema.Format != null && (schema.Format == "date" || schema.Format == "date-time"))
                    {
                        if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
                        {
                            return new OpenApiDate(dateValue.Date);
                        }
                    }
                    else if (schema.Format != null && schema.Format == "password")
                    {
                        return new OpenApiPassword(value);
                    }
                    else
                    {
                        return new OpenApiString(value);
                    }
                }
                if (schema.Type == "boolean")
                {
                    if (bool.TryParse(value, out var booleanValue))
                    {
                        return new OpenApiBoolean(booleanValue);
                    }
                }
            }
            return any;
        }
    }
}
