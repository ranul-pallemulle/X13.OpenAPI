using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;
using X13.OpenAPI.Public.Model.Types.RuntimeExpressions;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiLink : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public string OperationRef { get; set; }
        public string OperationId { get; set; }
        public IDictionary<string, OpenApiRuntimeExpressionOrAny> Parameters { get; set; } = new Dictionary<string, OpenApiRuntimeExpressionOrAny>();
        public OpenApiRuntimeExpressionOrAny RequestBody { get; set; }
        public string Description { get; set; }
        public OpenApiServer Server { get; set; }
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.Link;
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
    }

    public class OpenApiRuntimeExpressionOrAny : IOpenApiElement
    {
        public IOpenApiAny _any;
        public OpenApiRuntimeExpression _expression;

        public IOpenApiAny Any
        {
            get { return _any; }
            set
            {
                _expression = null;
                _any = value;
            }
        }
        public OpenApiRuntimeExpression Expression
        {
            get { return _expression; }
            set
            {
                _any = null;
                _expression = value;
            }
        }
    }
}
