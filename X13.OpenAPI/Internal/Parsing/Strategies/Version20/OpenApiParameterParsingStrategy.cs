using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
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

            // parse schema first
            var schemaNode = node.GetChildByName("schema");
            if (schemaNode != null)
            {
                element.Schema = schemaNode.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
            }
            else
            {
                element.Schema = new OpenApiSchema();
            }

            // parse other properties
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
                    case "type":
                        element.Schema.Type = childNode.GetSimpleValue<string>();
                        break;
                    case "format":
                        element.Schema.Format = childNode.GetSimpleValue<string>();
                        break;
                    case "allowEmptyValue":
                        element.AllowEmptyValue = childNode.GetSimpleValue<bool>();
                        break;
                    case "items":
                        element.Schema.Items = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiItemsParsingStrategy>();
                        break;
                    case "collectionFormat":
                        var format = childNode.GetSimpleValue<string>();
                        if (format == "csv")
                        {
                            if (element.In == OpenApiParameterLocation.Query)
                            {
                                element.Style = OpenApiParameterStyle.Form;
                            }
                            else
                            {
                                element.Style = OpenApiParameterStyle.Simple;
                            }
                        }
                        else if (format == "ssv")
                        {
                            element.Style = OpenApiParameterStyle.SpaceDelimited;
                        }
                        else if (format == "pipes")
                        {
                            element.Style = OpenApiParameterStyle.PipeDelimited;
                        }
                        else if (format == "multi")
                        {
                            element.Style = OpenApiParameterStyle.Form;
                            element.Explode = true;
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
