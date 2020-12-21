using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiResponses : Dictionary<string, OpenApiResponse>, IOpenApiExtensible, IOpenApiElement
    {
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
