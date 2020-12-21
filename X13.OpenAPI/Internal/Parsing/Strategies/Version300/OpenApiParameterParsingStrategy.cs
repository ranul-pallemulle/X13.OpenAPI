using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiParameterParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiParameter;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "name":
                        element.Name = childNode.GetSimpleValue<string>();
                        break;
                    case "in":
                        element.In = (OpenApiParameterLocation)Enum.Parse(typeof(OpenApiParameterLocation), childNode.GetSimpleValue<string>(), true);
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "required":
                        element.Required = childNode.GetSimpleValue<bool>();
                        break;
                    case "deprecated":
                        element.Deprecated = childNode.GetSimpleValue<bool>();
                        break;
                    case "allowEmptyValue":
                        element.AllowEmptyValue = childNode.GetSimpleValue<bool>();
                        break;
                    case "style":
                        element.Style = (OpenApiParameterStyle)Enum.Parse(typeof(OpenApiParameterStyle), childNode.GetSimpleValue<string>());
                        break;
                    case "explode":
                        element.Explode = childNode.GetSimpleValue<bool>();
                        break;
                    case "allowReserved":
                        element.AllowReserved = childNode.GetSimpleValue<bool>();
                        break;
                    case "schema":
                        element.Schema = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "content":
                        element.Content = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiMediaType, OpenApiMediaTypeParsingStrategy>();
                        break;
                    case "example":
                        // TODO
                        break;
                    case "examples":
                        element.Examples = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiExample, OpenApiExampleParsingStrategy>();
                        break;
                }
            }
        }
    }
}