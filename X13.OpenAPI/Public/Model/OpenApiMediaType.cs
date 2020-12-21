using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiMediaType : IOpenApiExtensible, IOpenApiElement
    {
        public OpenApiSchema Schema { get; set; }
        public IOpenApiAny Example { get; set; }
        public IDictionary<string, OpenApiExample> Examples { get; set; } = new Dictionary<string, OpenApiExample>();
        public IDictionary<string, OpenApiEncoding> Encoding { get; set; } = new Dictionary<string, OpenApiEncoding>();
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
