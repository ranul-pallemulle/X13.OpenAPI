using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Internal.Parsing.Services;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiOperationParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            ClearStorage(node);
            var element = parsingElement as OpenApiOperation;
            element.Extensions = new Dictionary<string, IOpenApiExtension>();

            // parse produces/consumes first
            var producesNode = node.GetChildByName("produces");
            if (producesNode != null)
            {
                var produces = (producesNode as ListParsingNode)
                            .CreateList(n => (string)(n as ValueParsingNode).Value);
                node.storage.Store("operation.produces", produces);
            }
            var consumesNode = node.GetChildByName("consumes");
            if (consumesNode != null)
            {
                var consumes = (consumesNode as ListParsingNode)
                            .CreateList(n => (string)(n as ValueParsingNode).Value);
                node.storage.Store("operation.consumes", consumes);
            }

            // parse other properties
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
                        var parameters = (childNode.Value as ListParsingNode).CreateList<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        node.storage.Store("operation.parameters", parameters);
                        break;
                    case "responses":
                        element.Responses = childNode.Value.ParseIntoElement<OpenApiResponses, OpenApiResponsesParsingStrategy>();
                        break;
                    case "schemes":
                        var schemes = (childNode.Value as ListParsingNode).CreateList(n => (string)(n as ValueParsingNode).Value);
                        node.storage.Store("operation.schemes", schemes);
                        break;
                    case "deprecated":
                        element.Deprecated = childNode.GetSimpleValue<bool>();
                        break;
                    case "security":
                        element.Security = (childNode.Value as ListParsingNode).CreateList<OpenApiSecurityRequirement, OpenApiSecurityRequirementParsingStrategy>();
                        break;
                }
            }

            element.RequestBody = OpenApiTransformer.BodyParametersToRequestBody(node);
            element.Parameters = OpenApiTransformer.NonBodyParameters(node);
            element.Servers = OpenApiTransformer.CreateServers(node, isOperation: true);

            ClearStorage(node);
        }

        private void ClearStorage(ParsingNode node)
        {
            node.storage.Remove("operation.consumes");
            node.storage.Remove("operation.produces");
            node.storage.Remove("operation.parameters");
            node.storage.Remove("operation.schemes");
            node.storage.Remove("operation.responses");
        }
    }
}
