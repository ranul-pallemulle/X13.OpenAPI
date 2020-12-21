using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiXmlParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiXml;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "name":
                        element.Name = childNode.GetSimpleValue<string>();
                        break;
                    case "namespace":
                        element.Namespace = childNode.GetSimpleValue<string>();
                        break;
                    case "prefix":
                        element.Prefix = childNode.GetSimpleValue<string>();
                        break;
                    case "attribute":
                        element.Attribute = childNode.GetSimpleValue<bool>();
                        break;
                    case "wrapped":
                        element.Wrapped = childNode.GetSimpleValue<bool>();
                        break;
                }
            }
        }
    }
}