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
    internal class OpenApiDocumentParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            ClearStorage(node);
            var element = parsingElement as OpenApiDocument;
            element.Extensions = new Dictionary<string, IOpenApiExtension>();

            // parse host, basepath and schemes
            var hostNode = node.GetChildByName("host");
            if (hostNode != null)
            {
                var host = (string)(hostNode as ValueParsingNode).Value;
                node.storage.Store("root.host", host);
            }
            var basePathNode = node.GetChildByName("basePath");
            if (basePathNode != null)
            {
                var basePath = (string)(basePathNode as ValueParsingNode).Value;
                node.storage.Store("root.basePath", basePath);
            }
            var schemesNode = node.GetChildByName("schemes");
            if (schemesNode != null)
            {
                var schemes = (schemesNode as ListParsingNode)
                            .CreateList(n => (string)(n as ValueParsingNode).Value);
                node.storage.Store("root.schemes", schemes);
            }

            // parse produces/consumes
            var producesNode = node.GetChildByName("produces");
            if (producesNode != null)
            {
                var produces = (producesNode as ListParsingNode)
                            .CreateList(n => (string)(n as ValueParsingNode).Value);
                node.storage.Store("root.produces", produces);
            }
            var consumesNode = node.GetChildByName("consumes");
            if (consumesNode != null)
            {
                var consumes = (consumesNode as ListParsingNode)
                            .CreateList(n => (string)(n as ValueParsingNode).Value);
                node.storage.Store("root.consumes", consumes);
            }

            // parse other properties
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "info":
                        element.Info = childNode.Value.ParseIntoElement<OpenApiInfo, OpenApiInfoParsingStrategy>();
                        break;
                    case "paths":
                        element.Paths = childNode.Value.ParseIntoElement<OpenApiPaths, OpenApiPathsParsingStrategy>();
                        break;
                    case "definitions":
                        var schemas = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        node.storage.Store("root.definitions", schemas);
                        break;
                    case "parameters":
                        var parameters = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiParameter, OpenApiParameterParsingStrategy>();
                        node.storage.Store("root.parameters", parameters);
                        break;
                    case "responses":
                        var responses = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiResponse, OpenApiResponseParsingStrategy>();
                        node.storage.Store("root.responses", responses);
                        break;
                    case "securityDefinitions":
                        var securityDefs = (childNode.Value as ObjectParsingNode)
                            .CreateObject<OpenApiSecurityScheme, OpenApiSecuritySchemeParsingStrategy>();
                        node.storage.Store("root.securityDefinitions", securityDefs);
                        break;
                    case "security":
                        element.Security = (childNode.Value as ListParsingNode)
                            .CreateList<OpenApiSecurityRequirement, OpenApiSecurityRequirementParsingStrategy>();
                        break;
                    case "tags":
                        element.Tags = (childNode.Value as ListParsingNode)
                            .CreateList<OpenApiTag, OpenApiTagParsingStrategy>();
                        break;
                    case "externalDocs":
                        element.ExternalDocs = childNode.Value.ParseIntoElement<OpenApiExternalDocs, OpenApiExternalDocsParsingStrategy>();
                        break;
                }
            }
            element.Servers = OpenApiTransformer.CreateServers(node, isOperation: false);
            element.Components = OpenApiTransformer.CreateComponents(node);
            ClearStorage(node);
        }

        private void ClearStorage(ParsingNode node)
        {
            node.storage.Remove("root.host");
            node.storage.Remove("root.basePath");
            node.storage.Remove("root.schemes");
            node.storage.Remove("root.produces");
            node.storage.Remove("root.consumes");
            node.storage.Remove("root.definitions");
            node.storage.Remove("root.parameters");
            node.storage.Remove("root.responses");
            node.storage.Remove("root.securityDefinitions");
        }
    }
}
