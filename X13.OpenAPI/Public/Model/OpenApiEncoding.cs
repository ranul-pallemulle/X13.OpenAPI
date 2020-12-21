using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiEncoding : IOpenApiExtensible, IOpenApiElement
    {
        public string ContentType { get; set; }
        public IDictionary<string, OpenApiHeader> Headers { get; set; } = new Dictionary<string, OpenApiHeader>();
        public OpenApiParameterStyle? Style { get; set; }
        public bool? Explode { get; set; }
        public bool? AllowReserved { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
