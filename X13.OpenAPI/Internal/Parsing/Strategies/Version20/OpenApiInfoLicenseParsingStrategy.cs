using System;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

internal class OpenApiInfoLicenseParsingStrategy : IOpenApiParsingStrategy
{
    public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
    {
        if (!(parsingNode is ObjectParsingNode node))
        {
            throw new ArgumentException();
        }
        var element = parsingElement as OpenApiLicense;
        foreach (var childNode in node.childNodes)
        {
            switch (childNode.Name)
            {
                case "name":
                    element.Name = childNode.GetSimpleValue<string>();
                    break;
                case "url":
                    element.Url = childNode.GetSimpleValue<string>();
                    break;
            }
        }
    }
}