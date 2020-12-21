using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Internal.Parsing.Context;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Services
{
    internal static class OpenApiTransformer
    {
        /// <summary>
        /// Creates an OpenAPI 3 RequestBody from a Swagger 2.0 body parameter.
        /// </summary>
        /// <param name="node"> Contains current parsing context - used for retrieving values from temporary storage. </param>
        /// <param name="bodyParam"> Body parameter to be converted. </param>
        /// <returns> Newly created RequestBody </returns>
        private static OpenApiRequestBody BodyParameterToRequestBody(ParsingNode node, OpenApiParameter bodyParam)
        {
            var requestBody = new OpenApiRequestBody();
            requestBody.Description = bodyParam.Description;
            requestBody.Required = bodyParam.Required;
            requestBody.Extensions = bodyParam.Extensions;
            if (bodyParam.Schema != null)
            {
                requestBody.Content = new Dictionary<string, OpenApiMediaType>();
                var consumes = node.storage.Retrieve<IList<string>>("operation.consumes");
                if (consumes == null)
                {
                    consumes = node.storage.Retrieve<IList<string>>("root.consumes");
                }
                if (consumes == null)
                {
                    consumes = new List<string> { "application/json" };
                }
                foreach (var mediaType in consumes)
                {
                    requestBody.Content.Add(mediaType, new OpenApiMediaType // maybe want to not replace keys?
                    {
                        Schema = bodyParam.Schema
                    });
                }
            }
            return requestBody;
        }

        public static OpenApiRequestBody BodyParametersToRequestBody(ParsingNode node)
        {
            var parameters = node.storage.Retrieve<IList<OpenApiParameter>>("operation.parameters");
            if (parameters == null)
            {
                return null;
            }
            var bodies = new List<OpenApiRequestBody>();
            foreach (var parameter in parameters)
            {
                if (parameter.In == OpenApiParameterLocation.Body || parameter.In == OpenApiParameterLocation.FormData)
                {
                    bodies.Add(BodyParameterToRequestBody(node, parameter));
                }
            }
            return MergeRequestBodies(bodies);
        }

        public static IList<OpenApiParameter> NonBodyParameters(ParsingNode node)
        {
            var parameters = node.storage.Retrieve<IList<OpenApiParameter>>("operation.parameters");
            if (parameters == null)
            {
                return new List<OpenApiParameter>();
            }
            var nonBodyParams = new List<OpenApiParameter>();
            foreach (var parameter in parameters)
            {
                if (parameter.In != OpenApiParameterLocation.Body && parameter.In != OpenApiParameterLocation.FormData)
                {
                    nonBodyParams.Add(parameter);
                }
            }
            return nonBodyParams;
        }

        private static OpenApiRequestBody MergeRequestBodies(IList<OpenApiRequestBody> requestBodies)
        {
            var result = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>()
            };
            foreach (var requestBody in requestBodies)
            {
                if (requestBody.Required)
                {
                    result.Required = true; // if any of the param requestBodies are required
                }
                if (!string.IsNullOrWhiteSpace(requestBody.Description))
                {
                    result.Description = requestBody.Description;
                }
                if (requestBody.Extensions != null && requestBody.Extensions.Count > 0)
                {
                    result.Extensions = requestBody.Extensions;
                }
                if (requestBody.Content != null)
                {
                    foreach (var content in requestBody.Content)
                    {
                        result.Content.Add(content);
                    }
                }
            }
            return result;
        }

        public static IList<OpenApiServer> CreateServers(ParsingNode node, bool isOperation)
        {
            var host = node.storage.Retrieve<string>("root.host");
            if (host == null)
            {
                throw new Exception("Host not found.");
            }
            var basePath = node.storage.Retrieve<string>("root.basePath");
            if (basePath == null)
            {
                basePath = string.Empty;
            }
            var schemes = node.storage.Retrieve<IList<string>>("operation.schemes");
            if (schemes == null)
            {
                if (isOperation)
                {
                    return new List<OpenApiServer>(); // no schemes specified - no need to override root servers
                }
                schemes = node.storage.Retrieve<IList<string>>("root.schemes");
            }
            if (schemes == null)
            {
                schemes = new List<string>();
            }
            if (schemes.Count == 0)
            {
                schemes.Add("http");
            }
            host = "://" + host;
            var servers = new List<OpenApiServer>();
            foreach (var scheme in schemes)
            {
                var url = scheme + host + basePath;
                servers.Add(new OpenApiServer
                {
                    Url = url
                });
            }
            return servers;
        }

        public static OpenApiComponents CreateComponents(ParsingNode node)
        {
            var components = new OpenApiComponents();
            var definitions = node.storage.Retrieve<IDictionary<string, OpenApiSchema>>("root.definitions");
            if (definitions != null)
            {
                components.Schemas = new Dictionary<string, OpenApiSchema>();
                foreach (var schema in definitions)
                {
                    components.Schemas.Add(schema);
                }
            }
            var parameters = node.storage.Retrieve<IDictionary<string, OpenApiParameter>>("root.parameters");
            if (parameters != null)
            {
                components.Parameters = new Dictionary<string, OpenApiParameter>();
                components.RequestBodies = new Dictionary<string, OpenApiRequestBody>();
                foreach (var parameter in parameters)
                {
                    if (parameter.Value.In == OpenApiParameterLocation.Body || parameter.Value.In == OpenApiParameterLocation.FormData)
                    {
                        components.RequestBodies.Add(parameter.Key, BodyParameterToRequestBody(node, parameter.Value));
                    }
                    else
                    {
                        components.Parameters.Add(parameter);
                    }
                }
            }
            var responses = node.storage.Retrieve<IDictionary<string, OpenApiResponse>>("root.responses");
            if (responses != null)
            {
                components.Responses = new OpenApiResponses();
                foreach (var response in responses)
                {
                    components.Responses.Add(response);
                }
            }
            var securityDefinitions = node.storage.Retrieve<IDictionary<string, OpenApiSecurityScheme>>("root.securityDefinitions");
            if (securityDefinitions != null)
            {
                components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>();
                foreach (var scheme in securityDefinitions)
                {
                    components.SecuritySchemes.Add(scheme);
                }
            }
            return components;
        }
    }
}
