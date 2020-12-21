using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiHeaderParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiHeader;
            element.Schema = new OpenApiSchema();
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "type":
                        var type = childNode.GetSimpleValue<string>();
                        node.storage.Store("header.type", type);
                        break;
                    case "format":
                        var format = childNode.GetSimpleValue<string>();
                        node.storage.Store("header.format", format);
                        break;
                    case "items":
                        element.Schema.Items = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiItemsParsingStrategy>();
                        break;
                    case "collectionFormat":
                        var collectionFormat = childNode.GetSimpleValue<string>();
                        if (collectionFormat == "csv")
                        {
                            element.Style = OpenApiParameterStyle.Simple;
                        }
                        else if (collectionFormat == "ssv")
                        {
                            element.Style = OpenApiParameterStyle.SpaceDelimited;
                        }
                        else if (collectionFormat == "pipes")
                        {
                            element.Style = OpenApiParameterStyle.PipeDelimited;
                        }
                        break;
                    case "default":
                        element.Schema.Default = childNode.Value.ParseIntoAny();
                        break;
                    case "maximum":
                        element.Schema.Maximum = childNode.GetSimpleValue<long>();
                        break;
                    case "exclusiveMaximum":
                        element.Schema.ExclusiveMaximum = childNode.GetSimpleValue<bool>();
                        break;
                    case "minimum":
                        element.Schema.Minimum = childNode.GetSimpleValue<long>();
                        break;
                    case "exclusiveMinimum":
                        element.Schema.ExclusiveMinimum = childNode.GetSimpleValue<bool>();
                        break;
                    case "maxLength":
                        element.Schema.MaxLength = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "minLength":
                        element.Schema.MinLength = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "pattern":
                        element.Schema.Pattern = childNode.GetSimpleValue<string>();
                        break;
                    case "maxItems":
                        element.Schema.MaxItems = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "minItems":
                        element.Schema.MinItems = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "uniqueItems":
                        element.Schema.UniqueItems = childNode.GetSimpleValue<bool>();
                        break;
                    case "enum":
                        // TODO
                        break;
                    case "multipleOf":
                        element.Schema.MultipleOf = childNode.GetSimpleValue<long>();
                        break;
                }
            }
        }
    }
}
