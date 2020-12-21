using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiOAuthFlow : IOpenApiExtensible, IOpenApiElement
    {
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public string RefreshUrl { get; set; }
        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
