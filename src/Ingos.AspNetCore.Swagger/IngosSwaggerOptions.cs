// -----------------------------------------------------------------------
// <copyright file= "SwaggerOptions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 18:23
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Ingos.AspNetCore.Swagger
{
    public class IngosSwaggerOptions
    {
        /// <summary>
        ///     Whether to enable swagger proxy.
        ///     If set to true, the swagger page of another service can be proxied
        /// </summary>
        public bool EnableProxy { get; set; } = true;

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