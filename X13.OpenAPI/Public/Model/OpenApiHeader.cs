using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiHeader : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
        public bool AllowEmptyValue { get; set; }
        public OpenApiParameterStyle? Style { get; set; }
        public bool? Explode { get; set; }
        public bool? AllowReserved { get; set; }
        public OpenApiSchema Schema { get; set; }
        public IOpenApiAny Example { get; set; }
        public IDictionary<string, OpenApiExample> Examples { get; set; } = new Dictionary<string, OpenApiExample>();
        public IDictionary<string, OpenApiMediaType> Content { get; set; } = new Dictionary<string, OpenApiMediaType>();
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.Header;
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }

    public enum OpenApiParameterStyle
    {
        Matrix,
        Label,
        Form,
        Simple,
        SpaceDelimited,
        PipeDelimited,
        DeepObject
    }
}
