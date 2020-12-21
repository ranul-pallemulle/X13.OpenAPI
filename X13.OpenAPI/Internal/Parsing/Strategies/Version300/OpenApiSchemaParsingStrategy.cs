using System;
using System.Collections.Generic;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Internal.Parsing.Services;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version300
{
    internal class OpenApiSchemaParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiSchema;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "title":
                        element.Title = childNode.GetSimpleValue<string>();
                        break;
                    case "type":
                        element.Type = childNode.GetSimpleValue<string>();
                        break;
                    case "format":
                        element.Format = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "maximum":
                        element.Maximum = childNode.GetSimpleValue<long>();
                        break;
                    case "minimum":
                        element.Minimum = childNode.GetSimpleValue<long>();
                        break;
                    case "exclusiveMaximum":
                        element.ExclusiveMaximum = childNode.GetSimpleValue<bool>();
                        break;
                    case "exclusiveMinimum":
                        element.ExclusiveMinimum = childNode.GetSimpleValue<bool>();
                        break;
                    case "maxLength":
                        element.MaxLength = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "minLength":
                        element.MinLength = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "pattern":
                        element.Pattern = childNode.GetSimpleValue<string>();
                        break;
                    case "multipleOf":
                        element.MultipleOf = childNode.GetSimpleValue<long>();
                        break;
                    case "default":
                        element.Default = childNode.Value.ParseIntoAny();
                        break;
                    case "readOnly":
                        element.ReadOnly = childNode.GetSimpleValue<bool>();
                        break;
                    case "writeOnly":
                        element.WriteOnly = childNode.GetSimpleValue<bool>();
                        break;
                    case "allOf":
                        element.AllOf = (childNode.Value as ListParsingNode).CreateList<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "oneOf":
                        element.OneOf = (childNode.Value as ListParsingNode).CreateList<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "anyOf":
                        element.AnyOf = (childNode.Value as ListParsingNode).CreateList<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "not":
                        element.Not = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "required":
                        element.Required = (childNode.Value as ListParsingNode).CreateList(n => (string)(n as ValueParsingNode).Value);
                        break;
                    case "items":
                        element.Items = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "maxItems":
                        element.MaxItems = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "minItems":
                        element.MinItems = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "uniqueItems":
                        element.UniqueItems = childNode.GetSimpleValue<bool>();
                        break;
                    case "properties":
                        element.Properties = (childNode.Value as ObjectParsingNode).CreateObject<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        break;
                    case "maxProperties":
                        element.MaxProperties = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "minProperties":
                        element.MinProperties = (int)childNode.GetSimpleValue<long>();
                        break;
                    case "additionalPropertiesAllowed":
                        element.AdditionalPropertiesAllowed = childNode.GetSimpleValue<bool>();
                        break;
                    case "additionalProperties":
                        if (childNode.Value is ValueParsingNode boolField)
                        {
                            element.AdditionalProperties = (bool)boolField.Value;
                        }
                        else
                        {
                            element.AdditionalProperties = childNode.Value.ParseIntoElement<OpenApiSchema, OpenApiSchemaParsingStrategy>();
                        }
                        break;
                    case "discriminator":
                        element.Discriminator = childNode.Value.ParseIntoElement<OpenApiDiscriminator, OpenApiDiscriminatorParsingStrategy>();
                        break;
                    case "example":
                        // TODO
                        break;
                    case "enum":
                        element.Enum = (childNode.Value.ParseIntoAny() as OpenApiArray);
                        break;
                    case "nullable":
                        element.Nullable = childNode.GetSimpleValue<bool>();
                        break;
                    case "externalDocs":
                        element.ExternalDocs = childNode.Value.ParseIntoElement<OpenApiExternalDocs, OpenApiExternalDocsParsingStrategy>();
                        break;
                    case "deprecated":
                        element.Deprecated = childNode.GetSimpleValue<bool>();
                        break;
                    case "xml":
                        element.Xml = childNode.Value.ParseIntoElement<OpenApiXml, OpenApiXmlParsingStrategy>();
                        break;
                }
            }

            // Resolve into type-specific OpenApiAny
            if (element.Default != null)
            {
                element.Default = OpenApiAnyTypeResolver.Resolve(element.Default, element);
            }
            if (element.Enum != null)
            {
                var newArray = new List<IOpenApiAny>();
                foreach (var item in element.Enum)
                {
                    newArray.Add(OpenApiAnyTypeResolver.Resolve(item, element));
                }
                element.Enum = newArray;
            }
        }
    }

    internal class OpenApiDiscriminatorParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiDiscriminator;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "propertyName":
                        element.PropertyName = childNode.GetSimpleValue<string>();
                        break;
                    case "mapping":
                        element.Mapping = (childNode.Value as ObjectParsingNode).CreateObject(n => (string)(n as ValueParsingNode).Value);
                        break;
                }
            }
        }
    }
}