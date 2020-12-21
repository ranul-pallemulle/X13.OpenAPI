using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiInfo : IOpenApiExtensible, IOpenApiElement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public OpenApiContact Contact { get; set; }
        public OpenApiLicense License { get; set; }
        public string Version { get; set; }

        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }
}
