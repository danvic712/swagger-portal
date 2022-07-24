// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerGenOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-24 16:40
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerGenOptions : SwaggerGenOptions
    {
        /// <summary>
        ///     Enable API versioning, the default value is true
        /// </summary>
        public bool EnableApiVersioning { get; set; } = true;

        /// <summary>
        ///     Open Api info
        /// </summary>
        public OpenApiInfo OpenApiInfo { get; set; } = new OpenApiInfo()
        {
            Title = "Ingos Backend API"
        };

        /// <summary>
        ///     The paths of api comment's xml
        /// </summary>
        public IList<string> ApiCommentPaths { get; set; } = new List<string>();
    }
}