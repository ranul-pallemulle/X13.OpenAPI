using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiExample : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public IOpenApiAny Value { get; set; }
        public string ExternalValue { get; set; }
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.Example;
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
