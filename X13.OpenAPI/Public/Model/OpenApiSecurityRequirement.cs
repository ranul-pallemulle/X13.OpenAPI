﻿using System;
using System.Collections.Generic;
using System.Text;
using X13.OpenAPI.Public.Model.Types;

namespace X13.OpenAPI.Public.Model
{
    public class OpenApiSecurityRequirement : Dictionary<string, IList<string>>, IOpenApiElement
    {
    }
}
