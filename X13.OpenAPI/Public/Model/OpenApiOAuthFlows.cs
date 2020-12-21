using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiOAuthFlows : IOpenApiExtensible, IOpenApiElement
    {
        public OpenApiOAuthFlow Implicit { get; set; }
        public OpenApiOAuthFlow Password { get; set; }
        public OpenApiOAuthFlow ClientCredentials { get; set; }
        public OpenApiOAuthFlow AuthorizationCode { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
