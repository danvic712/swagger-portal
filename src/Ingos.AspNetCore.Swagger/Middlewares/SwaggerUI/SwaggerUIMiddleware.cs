// -----------------------------------------------------------------------
// <copyright file= "SwaggerUIMiddleware.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 20:21
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ingos.AspNetCore.Swagger.Middlewares.SwaggerUI
{
    public class SwaggerUIMiddleware
    {
        private const string UINamespace = "Ingos.AspNetCore.SwaggerUI";

        public async Task InvokeAsync(HttpContext context)
        {
            Task.FromResult(0);
        }
    }
}

