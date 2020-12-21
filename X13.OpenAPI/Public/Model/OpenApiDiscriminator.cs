using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiDiscriminator : IOpenApiElement
    {
        public string PropertyName { get; set; }
        public IDictionary<string, string> Mapping { get; set; } = new Dictionary<string, string>();
    }
}
