using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model;
using X13.OpenAPI.Public.Services.Parsing;
using Xunit;

namespace X13.OpenAPI.Tests.Parsing.OpenApi3
{
    public class SimpleTests
    {
        [Fact]
        public void SimpleDocumentParsingTest_NoReferences()
        {
            var json = @"
            {
                ""openapi"": ""3.0.2"",
                ""info"": {
                    ""title"": ""Simple OpenAPI Document"",
                    ""version"": ""v1""
                },
                ""servers"": [
                    {
                        ""url"": ""https://simple.com""
                    }
                ],
                ""paths"": {
                    ""/path1"": {
                        ""get"": {
                            ""summary"": ""Simple GET Operation"",
                            ""operationId"": ""simpleGet"",
                            ""parameters"": [
                                {
                                    ""in"": ""query"",
                                    ""name"": ""param1"",
                                    ""required"": true,
                                    ""schema"": {
                                        ""type"": ""string""
                                    }
                                }
                            ],
                            ""requestBody"": {
                                ""required"": true,
                                ""content"": {
                                    ""application/json"": {
                                        ""schema"": {
                                            ""type"": ""object"",
                                            ""properties"": {
                                                ""name"": {
                                                    ""type"": ""string"",
                                                },
                                                ""age"": {
                                                    ""type"": ""integer"",
                                                    ""format"": ""int32""
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            ""responses"": {
                                ""200"": {
                                    ""description"": ""Ok""
                                }
                            }
                        }
                    }
                }
            }
            ";

            IOpenApiReader reader = new OpenApiReader();
            var document = reader.Read(json);

            Assert.NotNull(document.Info);
            Assert.NotNull(document.Paths);
            Assert.Equal("Simple OpenAPI Document", document.Info.Title);
            Assert.Equal("v1", document.Info.Version);
        }

        [Fact]
        public void SimpleDocumentParsingTest_WithReferences()
        {
            var json = @"
            {
                ""openapi"": ""3.0.2"",
                ""info"": {
                    ""title"": ""Simple OpenAPI Document"",
                    ""version"": ""v1""
                },
                ""servers"": [
                    {
                        ""url"": ""https://simple.com""
                    }
                ],
                ""paths"": {
                    ""/path1"": {
                        ""get"": {
                            ""summary"": ""Simple GET Operation"",
                            ""operationId"": ""simpleGet"",
                            ""parameters"": [
                                {
                                    ""in"": ""query"",
                                    ""name"": ""param1"",
                                    ""required"": true,
                                    ""schema"": {
                                        ""type"": ""string""
                                    }
                                }
                            ],
                            ""requestBody"": {
                                ""$ref"": ""#/components/requestBodies/Body1""
                            },
                            ""responses"": {
                                ""200"": {
                                    ""description"": ""Ok""
                                }
                            }
                        }
                    }
                },
                ""components"": {
                    ""requestBodies"": {
                        ""Body1"": {
                            ""required"": true,
                            ""content"": {
                                ""application/json"": {
                                    ""schema"": {
                                        ""type"": ""object"",
                                        ""properties"": {
                                            ""name"": {
                                                ""type"": ""string"",
                                            },
                                            ""age"": {
                                                ""type"": ""integer"",
                                                ""format"": ""int32""
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ";

            IOpenApiReader reader = new OpenApiReader();
            var document = reader.Read(json);

            Assert.NotNull(document.Paths["/path1"]);
            Assert.NotNull(document.Paths["/path1"].Operations[OpenApiOperationType.Get].RequestBody);
            var body = document.Paths["/path1"].Operations[OpenApiOperationType.Get].RequestBody;
            Assert.NotNull(body.Content["application/json"]);
            Assert.NotNull(body.Content["application/json"].Schema);;
            Assert.Equal("object", body.Content["application/json"].Schema.Type);
        }
    }
}
