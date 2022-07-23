// -----------------------------------------------------------------------
// <copyright file= "SwaggerUIMiddlewareExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 20:22
// Modified by:
// Description:
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;

namespace Ingos.AspNetCore.Swagger.Middlewares.SwaggerUI
{
    public static class SwaggerUIMiddlewareExtensions
    {
        /// <summary>
        ///     Use Swagger UI middleware
        /// </summary>
        /// <param name="builder">request pipeline. <see cref="IApplicationBuilder" /></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIngosSwaggerUI(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerUIMiddleware>();
        }
    }
}

