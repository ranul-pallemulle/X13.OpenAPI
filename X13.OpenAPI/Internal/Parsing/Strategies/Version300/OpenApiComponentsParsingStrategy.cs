using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiComponentsParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiComponents;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "schemas":
                        element.Schemas = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "responses":
                        element.Responses = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiResponse, OpenApiResponseParsingStrategy>();
                        break;
                    case "parameters":
                        element.Parameters = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        break;
                    case "examples":
                        element.Examples = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiExample, OpenApiExampleParsingStrategy>();
                        break;
                    case "requestBodies":
                        element.RequestBodies = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiRequestBody, OpenApiRequestBodyParsingStrategy>();
                        break;
                    case "headers":
                        element.Headers = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiHeader, OpenApiHeaderParsingStrategy>();
                        break;
                    case "securitySchemes":
                        element.SecuritySchemes = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiSecurityScheme, OpenApiSecuritySchemeParsingStrategy>();
                        break;
                    case "links":
                        element.Links = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiLink, OpenApiLinkParsingStrategy>();
                        break;
                    case "callbacks":
                        element.Callbacks = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiCallback, OpenApiCallbackParsingStrategy>();
                        break;
                }
            }
        }
    }
}