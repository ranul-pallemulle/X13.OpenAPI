using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiDocumentParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiDocument;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "info":
                        element.Info = childNode.Value.ParseIntoElement<OpenApiInfo, OpenApiInfoParsingStrategy>();
                        break;
                    case "servers":
                        element.Servers = (childNode.Value as ListParsingNode).CreateList<OpenApiServer, OpenApiServerParsingStrategy>();
                        break;
                    case "paths":
                        element.Paths = childNode.Value.ParseIntoElement<OpenApiPaths, OpenApiPathsParsingStrategy>();
                        break;
                    case "components":
                        element.Components = childNode.Value.ParseIntoElement<OpenApiComponents, OpenApiComponentsParsingStrategy>();
                        break;
                    case "security":
                        element.Security = (childNode.Value as ListParsingNode).CreateList<OpenApiSecurityRequirement, OpenApiSecurityRequirementParsingStrategy>();
                        break;
                    case "tags":
                        element.Tags = (childNode.Value as ListParsingNode).CreateList<OpenApiTag, OpenApiTagParsingStrategy>();
                        break;
                    case "externalDocs":
                        element.ExternalDocs = childNode.Value.ParseIntoElement<OpenApiExternalDocs, OpenApiExternalDocsParsingStrategy>();
                        break;
                }
            }
        }
    }
}