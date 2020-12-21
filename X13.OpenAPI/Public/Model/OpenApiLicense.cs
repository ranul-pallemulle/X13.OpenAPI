using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiLicense : IOpenApiExtensible, IOpenApiElement
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
