using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiInfoParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiInfo;
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "title":
                        element.Title = childNode.GetSimpleValue<string>();
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "termsOfService":
                        element.TermsOfService = childNode.GetSimpleValue<string>();
                        break;
                    case "contact":
                        element.Contact = childNode.Value.ParseIntoElement<OpenApiContact, OpenApiInfoContactParsingStrategy>();
                        break;
                    case "license":
                        element.License = childNode.Value.ParseIntoElement<OpenApiLicense, OpenApiInfoLicenseParsingStrategy>();
                        break;
                    case "version":
                        element.Version = childNode.GetSimpleValue<string>();
                        break;
                }
            }
        }
    }
}
