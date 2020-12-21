using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Internal.Parsing.Strategies.Version20
{
    internal class OpenApiSecuritySchemeParsingStrategy : IOpenApiParsingStrategy
    {
        public void Parse(ParsingNode parsingNode, IOpenApiElement parsingElement)
        {
            if (!(parsingNode is ObjectParsingNode node))
            {
                throw new ArgumentException();
            }
            var element = parsingElement as OpenApiSecurityScheme;

            element.Flows = new OpenApiOAuthFlows();

            // parse OAuth2 flows first
            var flowNode = node.GetChildByName("flow");
            if (flowNode != null)
            {
                var flow = (string)(flowNode as ValueParsingNode).Value;
                if (flow == "implicit")
                {
                    element.Flows.Implicit = new OpenApiOAuthFlow();
                }
                else if (flow == "password")
                {
                    element.Flows.Password = new OpenApiOAuthFlow();
                }
                else if (flow == "application")
                {
                    element.Flows.ClientCredentials = new OpenApiOAuthFlow();
                }
                else if (flow == "accessCode")
                {
                    element.Flows.AuthorizationCode = new OpenApiOAuthFlow();
                }
            }

            // parse other properties
            foreach (var childNode in node.childNodes)
            {
                switch (childNode.Name)
                {
                    case "type":
                        var type = childNode.GetSimpleValue<string>();
                        if (type == "basic")
                        {
                            element.Type = OpenApiSecuritySchemeType.Http;
                        }
                        else
                        {
                            element.Type = (OpenApiSecuritySchemeType)Enum.Parse(typeof(OpenApiSecuritySchemeType), childNode.GetSimpleValue<string>(), true);
                        }
                        break;
                    case "description":
                        element.Description = childNode.GetSimpleValue<string>();
                        break;
                    case "name":
                        element.Name = childNode.GetSimpleValue<string>();
                        break;
                    case "in":
                        element.In = (OpenApiParameterLocation)Enum.Parse(typeof(OpenApiParameterLocation), childNode.GetSimpleValue<string>(), true);
                        break;
                    case "authorizationUrl":
                        var authUrl = childNode.GetSimpleValue<string>();
                        if (element.Flows.Implicit != null)
                        {
                            element.Flows.Implicit.AuthorizationUrl = authUrl;
                        }
                        if (element.Flows.AuthorizationCode != null)
                        {
                            element.Flows.AuthorizationCode.AuthorizationUrl = authUrl;
                        }
                        break;
                    case "tokenUrl":
                        var tokenUrl = childNode.GetSimpleValue<string>();
                        if (element.Flows.Password != null)
                        {
                            element.Flows.Password.TokenUrl = tokenUrl;
                        }
                        if (element.Flows.AuthorizationCode != null)
                        {
                            element.Flows.AuthorizationCode.TokenUrl = tokenUrl;
                        }
                        break;
                    case "scopes":
                        var scopes = (childNode.Value as ObjectParsingNode).CreateObject(n => (string)(n as ValueParsingNode).Value);
                        if (element.Flows.Implicit != null)
                        {
                            element.Flows.Implicit.Scopes = scopes;
                        }
                        if (element.Flows.Password != null)
                        {
                            element.Flows.Password.Scopes = scopes;
                        }
                        if (element.Flows.ClientCredentials != null)
                        {
                            element.Flows.ClientCredentials.Scopes = scopes;
                        }
                        if (element.Flows.AuthorizationCode != null)
                        {
                            element.Flows.AuthorizationCode.Scopes = scopes;
                        }
                        break;
                }
            }
        }
    }
}
