using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Interfaces
{
    internal interface IOpenApiVersionParser
    {
        OpenApiDocument Parse(JObject json);
        OpenApiDocument ParsePartial(JObject json, string pathName, string operationName);
        void ResolveLocalReferences(OpenApiDocument document);
        OpenApiReference CreateReference(OpenApiReferenceType type, string refLink);
    }
}
