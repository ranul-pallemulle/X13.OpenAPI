using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Public.Services.Parsing
{
    public interface IOpenApiReader
    {
        OpenApiDocument Read(string input);
        OpenApiDocument ReadPartial(string input, string pathName, string methodName);
    }
}
