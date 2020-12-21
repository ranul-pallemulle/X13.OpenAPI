using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal class OpenApiVersion300Deserializer : IOpenApiVersionParser
    {
        public OpenApiReference CreateReference(OpenApiReferenceType type, string refLink)
        {
            throw new NotImplementedException();
        }

        public OpenApiDocument Parse(JObject json)
        {
            throw new NotImplementedException();
        }

        public OpenApiDocument ParsePartial(JObject json, string pathName, string operationName)
        {
            throw new NotImplementedException();
        }

        public void ResolveLocalReferences(OpenApiDocument document)
        {
            throw new NotImplementedException();
        }
    }
}
