using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Internal.Parsing.Strategies.Version20;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal class OpenApiVersion20Deserializer : IOpenApiVersionParser
    {
        public OpenApiReference CreateReference(OpenApiReferenceType type, string refLink)
        {
            if (string.IsNullOrWhiteSpace(refLink))
            {
                throw new ArgumentNullException();
            }
            var segments = refLink.Split('#');
            if (segments.Length == 1 && (type == OpenApiReferenceType.Tag || type == OpenApiReferenceType.SecurityScheme))
            {
                return new OpenApiReference
                {
                    Type = type,
                    Id = refLink
                };
            }
            else if (segments.Length == 2)
            {
                if (!refLink.StartsWith("#"))
                {
                    throw new NotImplementedException("External references not yet supported");
                }
                var pathSegments = segments[1].Split('/');
                if (pathSegments.Length > 2)
                {
                    var reference = GetReferenceType(pathSegments[1]);
                    var index = refLink.IndexOf(pathSegments[2]);
                    return new OpenApiReference
                    {
                        Type = reference,
                        Id = refLink.Substring(index)
                    };
                }
            }
            throw new ArgumentException("Invalid reference link");
        }

        public OpenApiDocument Parse(JObject json)
        {
            var rootNode = ParsingNode.Create(json, this, new ContextStorage());
            var document = new OpenApiDocument();
            rootNode.strategy = new OpenApiDocumentParsingStrategy();
            rootNode.Parse(document);
            return document;
        }

        public OpenApiDocument ParsePartial(JObject json, string pathName, string operationName)
        {
            // TODO: implement partial parse for swagger v2.0 - for now do a full parse
            return Parse(json);
        }

        public void ResolveLocalReferences(OpenApiDocument document)
        {
            var traverser = new OpenApiTraverser(new OpenApiReferenceResolver(document));
            traverser.Traverse(document);
        }

        private OpenApiReferenceType GetReferenceType(string name)
        {
            switch (name)
            {
                case "definitions":
                    return OpenApiReferenceType.Schema;
                case "parameters":
                    return OpenApiReferenceType.Parameter;
                case "responses":
                    return OpenApiReferenceType.Response;
                case "headers":
                    return OpenApiReferenceType.Header;
                case "tags":
                    return OpenApiReferenceType.Tag;
                case "securityDefinitions":
                    return OpenApiReferenceType.SecurityScheme;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
