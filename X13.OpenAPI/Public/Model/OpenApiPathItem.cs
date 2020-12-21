using System.Collections.Generic;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiPathItem : IOpenApiExtensible, IOpenApiElement
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public IDictionary<OpenApiOperationType, OpenApiOperation> Operations { get; set; } = new Dictionary<OpenApiOperationType, OpenApiOperation>();
        public IList<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();
        public IList<OpenApiParameter> Parameters { get; set; } = new List<OpenApiParameter>();
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
        public void AddOperation(OpenApiOperationType operationType, OpenApiOperation operation)
        {
            Operations[operationType] = operation;
        }
    }
}