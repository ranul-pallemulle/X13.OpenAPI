using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types.Primitives
{
    public interface IOpenApiPrimitive : IOpenApiAny
    {
        OpenApiPrimitiveType PrimitiveType { get; }
    }

    public enum OpenApiPrimitiveType
    {
        Int32 = 0,
        Int64 = 1,
        Float = 2,
        Double = 3,
        String = 4,
        Byte = 5,
        Binary = 6,
        Boolean = 7,
        Date = 8,
        DateTime = 9,
        Password = 10
    }
}
