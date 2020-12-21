using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types
{
    public interface IOpenApiReferenceable
    {
        bool UnresolvedReference { get; set; }
        OpenApiReference Reference { get; set; }
        OpenApiReferenceType ReferenceType { get; }
    }
}
