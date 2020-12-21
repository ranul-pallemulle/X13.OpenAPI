using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiServerVariable : IOpenApiExtensible, IOpenApiElement
    {
        public IList<string> Enum { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
