using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiRequestBodyParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiRequestBody;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "required":
                        element.Required = childNode.GetSimpleValue<bool>();
                        break;
                    case "content":
                        element.Content = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiMediaType, OpenApiMediaTypeParsingStrategy>();
                        break;
                }
            }
        }
    }
}