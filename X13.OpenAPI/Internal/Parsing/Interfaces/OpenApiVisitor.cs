using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model;

namespace X13.OpenAPI.Internal.Parsing.Interfaces
{
    internal abstract class OpenApiVisitor
    {
        public virtual void Visit(OpenApiDocument doc)
        {

        }
        public virtual void Visit(OpenApiInfo info)
        {

        }
        public virtual void Visit(OpenApiContact contact)
        {

        }
        public virtual void Visit(OpenApiLicense license)
        {

        }
        public virtual void Visit(OpenApiServer server)
        {

        }
        public virtual void Visit(OpenApiServerVariable license)
        {

        }
        public virtual void Visit(OpenApiPaths paths)
        {

        }
        public virtual void Visit(OpenApiPathItem pathItem)
        {

        }
        public virtual void Visit(OpenApiOperation operation)
        {

        }
        public virtual void Visit(OpenApiRequestBody requestBody)
        {

        }
        public virtual void Visit(OpenApiMediaType mediaType)
        {

        }
        public virtual void Visit(OpenApiSchema schema)
        {

        }
        public virtual void Visit(OpenApiExample example)
        {

        }
        public virtual void Visit(OpenApiEncoding encoding)
        {

        }
        public virtual void Visit(OpenApiResponses responses)
        {

        }
        public virtual void Visit(OpenApiResponse response)
        {

        }
        public virtual void Visit(OpenApiLink link)
        {

        }
        public virtual void Visit(OpenApiHeader header)
        {

        }
        public virtual void Visit(OpenApiCallback callback)
        {

        }
        public virtual void Visit(OpenApiParameter parameter)
        {

        }
        public virtual void Visit(OpenApiComponents components)
        {

        }
        public virtual void Visit(OpenApiSecurityRequirement securityRequirement)
        {

        }
        public virtual void Visit(OpenApiTag tag)
        {

        }
        public virtual void Visit(OpenApiSecurityScheme scheme)
        {

        }
        public virtual void Visit(OpenApiOAuthFlows flows)
        {

        }
        public virtual void Visit(OpenApiOAuthFlow flow)
        {

        }
        public virtual void Visit(OpenApiExternalDocs externalDocs)
        {

        }
        public virtual void Visit(OpenApiXml xml)
        {

        }
    }
}
