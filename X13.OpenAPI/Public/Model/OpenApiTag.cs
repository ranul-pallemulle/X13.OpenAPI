using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiTag : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public OpenApiExternalDocs ExternalDocs { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; set; } = OpenApiReferenceType.Tag;
    }
}
