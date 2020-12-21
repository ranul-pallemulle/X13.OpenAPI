using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;
using X13.OpenAPI.Public.Model.Types.RuntimeExpressions;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiResponsesParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiResponses;
            foreach (var childNode in node.childNodes)
            {
                var response = childNode.Value.ParseIntoElement<OpenApiResponse, OpenApiResponseParsingStrategy>();
                element.Add(childNode.Name, response);
            }
        }
    }

    internal class OpenApiResponseParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiResponse;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "headers":
                        element.Headers = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiHeader, OpenApiHeaderParsingStrategy>();
                        break;
                    case "content":
                        element.Content = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiMediaType, OpenApiMediaTypeParsingStrategy>();
                        break;
                    case "links":
                        element.Links = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiLink, OpenApiLinkParsingStrategy>();
                        break;
                }
            }
        }
    }

    internal class OpenApiLinkParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiLink;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "operationRef":
                        element.OperationRef = childNode.GetSimpleValue<string>();
                        break;
                    case "operationId":
                        element.OperationId = childNode.GetSimpleValue<string>();
                        break;
                    case "parameters":
                        element.Parameters = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiRuntimeExpressionOrAny, OpenApiRuntimeExpressionAnyParsingStrategy>();
                        break;
                    case "requestBody":
                        // TODO
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "server":
                        element.Server = childNode.Value.ParseIntoElement<OpenApiServer, OpenApiServerParsingStrategy>();
                        break;
                }
            }
        }
    }

    internal class OpenApiRuntimeExpressionAnyParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ValueParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiRuntimeExpressionOrAny;
            var expr = OpenApiRuntimeExpression.Build((string)node.Value);
            element.Expression = expr;
        }
    }
}