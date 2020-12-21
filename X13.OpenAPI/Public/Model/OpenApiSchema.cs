using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiSchema : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public long? Maximum { get; set; }
        public long? Minimum { get; set; }
        public bool? ExclusiveMaximum { get; set; }
        public bool? ExclusiveMinimum { get; set; }
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public string Pattern { get; set; }
        public long? MultipleOf { get; set; }
        public IOpenApiAny Default { get; set; }
        public bool ReadOnly { get; set; }
        public bool WriteOnly { get; set; }
        public IList<OpenApiSchema> AllOf { get; set; } = new List<OpenApiSchema>();
        public IList<OpenApiSchema> OneOf { get; set; } = new List<OpenApiSchema>();
        public IList<OpenApiSchema> AnyOf { get; set; } = new List<OpenApiSchema>();
        public OpenApiSchema Not { get; set; }
        public IList<string> Required { get; set; }
        public OpenApiSchema Items { get; set; }
        public int? MaxItems { get; set; }
        public int? MinItems { get; set; }
        public bool? UniqueItems { get; set; }
        public IDictionary<string, OpenApiSchema> Properties { get; set; } = new Dictionary<string, OpenApiSchema>();
        public int? MaxProperties { get; set; }
        public int? MinProperties { get; set; }
        public bool AdditionalPropertiesAllowed { get; set; }
        public object AdditionalProperties { get; set; }
        public OpenApiDiscriminator Discriminator { get; set; }
        public IOpenApiAny Example { get; set; }
        public IList<IOpenApiAny> Enum { get; set; } = new List<IOpenApiAny>();
        public bool Nullable { get; set; }
        public OpenApiExternalDocs ExternalDocs { get; set; }
        public bool Deprecated { get; set; }
        public OpenApiXml Xml { get; set; }
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.Schema;
    }
}
