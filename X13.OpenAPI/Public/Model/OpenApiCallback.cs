using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiCallback : IOpenApiReferenceable, IOpenApiExtensible, IOpenApiElement
    {
        public Dictionary<object, OpenApiPathItem> PathItems { get; set; } = new Dictionary<object, OpenApiPathItem>();
        public bool UnresolvedReference { get; set; }
        public OpenApiReference Reference { get; set; }
        public OpenApiReferenceType ReferenceType { get; } = OpenApiReferenceType.Callback;
        public IDictionary<string, IOpenApiExtension> Extensions { get; set; } = new Dictionary<string, IOpenApiExtension>();
        public void AddPathItem(object expression , OpenApiPathItem pathItem)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (pathItem == null)
            {
                throw new ArgumentNullException(nameof(pathItem));
            }
            if (PathItems == null)
            {
                PathItems = new Dictionary<object, OpenApiPathItem>();
            }
            PathItems.Add(expression, pathItem);
        }
    }
}
