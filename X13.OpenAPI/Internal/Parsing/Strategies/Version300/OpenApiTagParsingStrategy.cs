using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiTagParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiTag;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "name":
                        element.Name = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "externalDocs":
                        element.ExternalDocs = childNode.Value.ParseIntoElement<OpenApiExternalDocs, OpenApiExternalDocsParsingStrategy>();
                        break;
                }
            }
        }
    }
}