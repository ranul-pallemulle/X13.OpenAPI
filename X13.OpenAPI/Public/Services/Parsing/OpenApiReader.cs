using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Internal.Parsing.Services;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Public.Services.Parsing
{
    public class OpenApiReader : IOpenApiReader
    {
        public OpenApiDocument Read(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Null or empty input");
            }
            var json = JObject.Parse(input);
            IOpenApiVersionParser deserializer = GetVersionParser(json);
            var document = deserializer.Parse(json);
            deserializer.ResolveLocalReferences(document);
            return document;
        }

        public OpenApiDocument ReadPartial(string input, string pathName, string methodName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Null or empty input");
            }
            var json = JObject.Parse(input);
            IOpenApiVersionParser deserializer = GetVersionParser(json);
            var document = deserializer.ParsePartial(json, pathName, methodName);
            deserializer.ResolveLocalReferences(document);
            return document;
        }

        private IOpenApiVersionParser GetVersionParser(JObject json)
        {
            IOpenApiVersionParser deserializer;
            var v2 = json.SelectToken("swagger");
            var v3 = json.SelectToken("openapi");
            if (v2 != null)
            {
                deserializer = new OpenApiVersion20Deserializer();
            }
            else if (v3 != null)
            {
                deserializer = new OpenApiVersion300Deserializer();
            }
            else
            {
                throw new ArgumentException("Invalid version information");
            }
            return deserializer;
        }
    }
}
