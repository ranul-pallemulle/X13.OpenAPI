using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Interfaces
{
    internal interface IOpenApiParsingStrategy
    {
        void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement);
    }
}
