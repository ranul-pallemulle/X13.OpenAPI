using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiXml : IOpenApiExtensible, IOpenApiElement
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Prefix { get; set; }
        public bool Attribute { get; set; }
        public bool Wrapped { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
