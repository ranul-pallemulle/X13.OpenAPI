using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types
{
    public class OpenApiObject : Dictionary<string, IOpenApiAny> , IOpenApiAny
    {
        public OpenApiAnyType AnyType => OpenApiAnyType.Object;
        public string GetString()
        {
            return this.ToString(); // TODO: maybe do something better here
        }
    }
}
