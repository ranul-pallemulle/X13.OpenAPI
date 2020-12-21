using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types
{
    public class OpenApiNull : IOpenApiAny
    {
        public OpenApiAnyType AnyType => OpenApiAnyType.Null;
        public string GetString()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return null;
        }
    }
}
