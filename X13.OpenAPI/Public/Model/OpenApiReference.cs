using System;
using System.Collections.Generic;
using System.Text;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiReference
    {
        public OpenApiReferenceType? Type { get; set; }
        public string Id { get; set; }
        public static string GetDisplayNameV3(OpenApiReferenceType type)
        {
            switch (type)
            {
                case (OpenApiReferenceType.Callback): return "callbacks";
                case (OpenApiReferenceType.Example): return "examples";
                case (OpenApiReferenceType.Header): return "headers";
                case (OpenApiReferenceType.Link): return "links";
                case (OpenApiReferenceType.Parameter): return "parameters";
                case (OpenApiReferenceType.RequestBody): return "requestBodies";
                case (OpenApiReferenceType.Response): return "responses";
                case (OpenApiReferenceType.Schema): return "schemas";
                case (OpenApiReferenceType.SecurityScheme): return "securitySchemes";
                case (OpenApiReferenceType.Tag): return "tags";
                default: return null;

            }
        }

        public static string GetDisplayNameV2(OpenApiReferenceType type)
        {
            switch (type)
            {
                case (OpenApiReferenceType.Schema): return "definitions";
                case (OpenApiReferenceType.Parameter): return "parameters";
                case (OpenApiReferenceType.Response): return "responses";
                case (OpenApiReferenceType.Header): return "headers";
                case (OpenApiReferenceType.Tag): return "tags";
                case (OpenApiReferenceType.SecurityScheme): return "securityDefinitions";
                default: return null;
            }
        }
    }

    public enum OpenApiReferenceType
    {
        Schema,
        Response,
        Parameter,
        Example,
        RequestBody,
        Header,
        SecurityScheme,
        Link,
        Callback,
        Tag
    }
}
