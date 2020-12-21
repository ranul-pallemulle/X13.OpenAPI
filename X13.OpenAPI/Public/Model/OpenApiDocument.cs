using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiDocument : IOpenApiExtensible, IOpenApiElement
    {
        public OpenApiInfo Info { get; set; }
        public IList<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();
        public OpenApiPaths Paths { get; set; }
        public OpenApiComponents Components { get; set; }
        public IList<OpenApiSecurityRequirement> Security { get; set; } = new List<OpenApiSecurityRequirement>();
        public IList<OpenApiTag> Tags { get; set; } = new List<OpenApiTag>();
        public OpenApiExternalDocs ExternalDocs { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();

        public IOpenApiReferenceable ResolveReference(OpenApiReference reference)
        {
            if (reference == null)
            {
                return null;
            }
            if (reference.Type == OpenApiReferenceType.Tag)
            {
                foreach (var tag in Tags)
                {
                    if (tag.Name == reference.Id)
                    {
                        tag.Reference = reference;
                        return tag;
                    }
                }
            }
            switch (reference.Type)
            {
                case OpenApiReferenceType.Schema:
                    return this.Components.Schemas[reference.Id];
                case OpenApiReferenceType.Response:
                    return this.Components.Responses[reference.Id];
                case OpenApiReferenceType.Parameter:
                    return this.Components.Parameters[reference.Id];
                case OpenApiReferenceType.Example:
                    return this.Components.Examples[reference.Id];
                case OpenApiReferenceType.RequestBody:
                    return this.Components.RequestBodies[reference.Id];
                case OpenApiReferenceType.Header:
                    return this.Components.Headers[reference.Id];
                case OpenApiReferenceType.SecurityScheme:
                    return this.Components.SecuritySchemes[reference.Id];
                case OpenApiReferenceType.Link:
                    return this.Components.Links[reference.Id];
                case OpenApiReferenceType.Callback:
                    return this.Components.Callbacks[reference.Id];
                default:
                    throw new ArgumentException("Invalid reference");
            }
        }
    }
}
