// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerGenOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-24 16:40
// Modified by:
// Description: Ingos Swagger Document Generate Configuration Options
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerGenOptions : SwaggerGenOptions
    {
        /// <summary>
        ///     Open Api info
        /// </summary>
        public OpenApiDescriptor OpenApiDescriptor = new OpenApiDescriptor();

        /// <summary>
        ///     Whether to enable API versioning, the default value is true.
        ///     If set to true, it will set api version
        /// </summary>
        public bool ApiVersioning { get; set; } = true;

        /// <summary>
        ///     The paths of API comment's xml
        /// </summary>
        public IList<string> XmlCommentPaths { get; set; } = new List<string>();
    }

    public class OpenApiDescriptor
    {
        /// <summary>
        ///     Global metadata to be included in the Swagger output
        /// </summary>
        public OpenApiInfo OpenApiInfo = new OpenApiInfo
        {
            Title = Assembly.GetCallingAssembly().GetName().Name
        };

        /// <summary>
        ///     A URI-friendly name that uniquely identifies the document
        /// </summary>
        public string Name { get; set; } = "v1";
    }
}