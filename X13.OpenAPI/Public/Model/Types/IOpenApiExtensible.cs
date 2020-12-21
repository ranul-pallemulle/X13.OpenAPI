using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model.Types
{
    public interface IOpenApiExtensible
    {
        IDictionary<string, IOpenApiExtension> Extensions { get; set; }
    }
}
