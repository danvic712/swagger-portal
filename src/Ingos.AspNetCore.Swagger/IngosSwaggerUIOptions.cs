// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerUIOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 18:23
// Modified by:
// Description: Ingos Swagger UI Configuration Options
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerUIOptions : SwaggerUIOptions
    {
        /// <summary>
        ///     Whether to enable swagger proxy, the default value is true.
        ///     If set to true, the swagger page of another service can be proxied.
        /// </summary>
        public bool HubMode { get; set; } = true;

        /// <summary>
        /// Gets or sets a route prefix for accessing the swagger hub ui
        /// </summary>
        public string HubRoutePrefix { get; set; } = "swagger-hub";

        /// <summary>
        ///     Gets or sets a title for the swagger hub ui page
        /// </summary>
        public string HubDocumentTitle { get; set; } = "Swagger Hub";

        /// <summary>
        ///     Swagger generated files.
        /// </summary>
        public IList<IngosSwaggerDocumentDescriptor> Documents { get; set; } =
            new List<IngosSwaggerDocumentDescriptor>();
    }

    public class IngosSwaggerDocumentDescriptor
    {
        /// <summary>
        ///     Generated Swagger file path
        ///     eg. /swagger/v1/swagger.json
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     The display order, default load the latest version
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     The document display name
        /// </summary>
        public string Name { get; set; }
    }
}