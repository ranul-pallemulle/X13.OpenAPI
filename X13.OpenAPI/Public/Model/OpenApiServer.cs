using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiServer : IOpenApiExtensible, IOpenApiElement
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public IDictionary<string, OpenApiServerVariable> Variables { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
