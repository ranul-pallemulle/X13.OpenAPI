using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiOperationParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiOperation;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "tags":
                        element.Tags = (childNode.Value as ListParsingNode).CreateList(n => (string)(n as ValueParsingNode).Value);
                        break;
                    case "summary":
                        element.Summary = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "externalDocs":
                        element.ExternalDocs = childNode.Value.ParseIntoElement<OpenApiExternalDocs, OpenApiExternalDocsParsingStrategy>();
                        break;
                    case "operationId":
                        element.OperationId = childNode.GetSimpleValue<string>();
                        break;
                    case "parameters":
                        element.Parameters = (childNode.Value as ListParsingNode).CreateList<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        break;
                    case "requestBody":
                        element.RequestBody = childNode.Value.ParseIntoElement<OpenApiRequestBody, OpenApiRequestBodyParsingStrategy>();
                        break;
                    case "responses":
                        element.Responses = childNode.Value.ParseIntoElement<OpenApiResponses, OpenApiResponsesParsingStrategy>();
                        break;
                    case "callbacks":
                        element.Callbacks = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiCallback, OpenApiCallbackParsingStrategy>();
                        break;
                    case "deprecated":
                        element.Deprecated = childNode.GetSimpleValue<bool>();
                        break;
                    case "security":
                        element.Security = (childNode.Value as ListParsingNode).CreateList<OpenApiSecurityRequirement, OpenApiSecurityRequirementParsingStrategy>();
                        break;
                    case "servers":
                        element.Servers = (childNode.Value as ListParsingNode).CreateList<OpenApiServer, OpenApiServerParsingStrategy>();
                        break;
                }
            }
        }
    }
}