using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiOperation : IOpenApiExtensible, IOpenApiElement
    {
        public const bool DeprecatedDefault = false;
        public IList<string> Tags { get; set; } = new List<string>();
        public string Summary { get; set; }
        public string Description { get; set; }
        public OpenApiExternalDocs ExternalDocs { get; set; }
        public string OperationId { get; set; }
        public IList<OpenApiParameter> Parameters { get; set; } = new List<OpenApiParameter>();
        public OpenApiRequestBody RequestBody { get; set; }
        public OpenApiResponses Responses { get; set; } = new OpenApiResponses();
        public IDictionary<string, OpenApiCallback> Callbacks { get; set; } = new Dictionary<string, OpenApiCallback>();
        public bool Deprecated { get; set; } = DeprecatedDefault;
        public IList<OpenApiSecurityRequirement> Security { get; set; } = new List<OpenApiSecurityRequirement>();
        public IList<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }

    public enum OpenApiOperationType
    {
        Get,
        Put,
        Post,
        Delete,
        Options,
        Head,
        Patch,
        Trace
    }
}
