// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-30 18:16
// Modified by:
// Description: Ingos Swagger Configuration Options
// -----------------------------------------------------------------------

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerOptions
    {
        /// <summary>
        /// Swagger Generate Options
        /// </summary>
        public IngosSwaggerGenOptions SwaggerGenOptions { get; set; } = new IngosSwaggerGenOptions();

        /// <summary>
        /// Swagger UI Options
        /// </summary>
        public IngosSwaggerUIOptions SwaggerUIOptions { get; set; } = new IngosSwaggerUIOptions();
    }
}