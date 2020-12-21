using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiSecurityScheme : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public OpenApiSecuritySchemeType Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public OpenApiParameterLocation In { get; set; }
        public string Scheme { get; set; }
        public string BearerFormat { get; set; }
        public OpenApiOAuthFlows Flows { get; set; }
        public string OpenIdConnectUrl { get; set; }
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.SecurityScheme;
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }

    public enum OpenApiSecuritySchemeType
    {
        ApiKey,
        Http,
        OAuth2,
        OpenIdConnect
    }
}
