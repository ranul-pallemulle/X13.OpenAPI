using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Internal.Parsing.Interfaces;
using X13.OpenAPI.Internal.Parsing.Strategies.Version300;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal class OpenApiVersion300Deserializer : IOpenApiVersionParser
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
                if (pathSegments.Length == 4 && pathSegments[1] == "components")
                {
                    var reference = GetReferenceType(pathSegments[2]);
                    return new OpenApiReference
                    {
                        Type = reference,
                        Id = pathSegments[3]
                    };
                }
            }
            throw new ArgumentException("Invalid reference link");
        }

        public OpenApiDocument Parse(JObject json)
        {
            var rootNode = ParsingNode.Create(json, this);
            var document = new OpenApiDocument();
            rootNode.strategy = new OpenApiDocumentParsingStrategy();
            rootNode.Parse(document);
            return document;
        }

        public OpenApiDocument ParsePartial(JObject json, string pathName, string operationName)
        {
            var rootNode = ParsingNode.Create(json, this);
            var document = new OpenApiDocument();
            rootNode.strategy = new OpenApiDocumentPartialParsingStrategy(pathName, operationName);
            rootNode.Parse(document);
            return document;
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
                case "schemas":
                    return OpenApiReferenceType.Schema;
                case "responses":
                    return OpenApiReferenceType.Response;
                case "parameters":
                    return OpenApiReferenceType.Parameter;
                case "examples":
                    return OpenApiReferenceType.Example;
                case "requestBodies":
                    return OpenApiReferenceType.RequestBody;
                case "headers":
                    return OpenApiReferenceType.Header;
                case "securitySchemes":
                    return OpenApiReferenceType.SecurityScheme;
                case "links":
                    return OpenApiReferenceType.Link;
                case "callbacks":
                    return OpenApiReferenceType.Callback;
                case "tags":
                    return OpenApiReferenceType.Tag;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
