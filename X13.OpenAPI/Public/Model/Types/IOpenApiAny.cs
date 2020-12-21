using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types
{
    public interface IOpenApiAny : IOpenApiElement, IOpenApiExtension
    {
        OpenApiAnyType AnyType { get; }
        string GetString();
    }

    public enum OpenApiAnyType
    {
        Primitive = 0,
        Null = 1,
        Array = 2,
        Object = 3
    }
}
