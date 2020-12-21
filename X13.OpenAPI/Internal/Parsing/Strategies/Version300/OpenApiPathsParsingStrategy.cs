using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiPathsParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiPaths;
            foreach (var childNode in node.childNodes)
            {
                if (childNode.Name.StartsWith("/")) // a path
                {
                    var pathItem = new OpenApiPathItem();
                    childNode.Value.strategy = new OpenApiPathItemParsingStrategy();
                    childNode.Value.Parse(pathItem);
                    element.Add(childNode.Name, pathItem);
                }
            }
        }
    }
}