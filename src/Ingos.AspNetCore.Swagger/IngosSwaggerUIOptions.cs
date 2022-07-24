// -----------------------------------------------------------------------
// <copyright file= "SwaggerUIOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 20:32
// Modified by:
// Description:
// -----------------------------------------------------------------------

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerUIOptions
    {
        /// <summary>
        ///     Gets or sets a route prefix for accessing the swagger-ui
        /// </summary>
        public string RoutePrefix { get; set; } = "swagger-hub";

        /// <summary>
        ///     Gets or sets a title for the swagger-ui page
        /// </summary>
        public string DocumentTitle { get; set; } = "Swagger Hub";
    }
}