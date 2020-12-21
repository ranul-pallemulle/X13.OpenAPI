using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;
using X13.OpenAPI.Public.Model.Types.RuntimeExpressions;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiCallbackParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiCallback;
            foreach (var childNode in node.childNodes)
            {
                var pathItemExpr = childNode.Name;
                var pathItem = childNode.Value.ParseIntoElement<OpenApiPathItem, OpenApiPathItemParsingStrategy>();
                element.PathItems.Add(OpenApiRuntimeExpression.Build(pathItemExpr), pathItem);
            }
        }
    }
}