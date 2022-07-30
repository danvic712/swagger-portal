// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerBuilderExtension.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 18:12
// Modified by:
// Description: Application Builder Extension Methods
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Ingos.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class IngosSwaggerBuilderExtension
    {
        /// <summary>
        ///     Register the Swagger middleware with provided options into the HTTP request pipeline
        /// </summary>
        public static IApplicationBuilder UseIngosSwagger(this IApplicationBuilder app, IngosSwaggerUIOptions options)
        {
            // Configure Swagger middleware
            //
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                // Default load the latest version
                foreach (var description in options.Documents.OrderByDescending(i => i.Order))
                    s.SwaggerEndpoint(description.Path, description.Name);
            });

            // Configure Yarp middleware
            if (options.HubMode)
                app.UseIngosSwaggerProxies(options);

            return app;
        }

        private static IApplicationBuilder UseIngosSwaggerProxies(this IApplicationBuilder app,
            IngosSwaggerUIOptions ingosSwaggerOptions)
        {
            return app;
        }

        /// <summary>
        ///     Register the Swagger middleware with optional setup action for DI-injected options
        /// </summary>
        public static IApplicationBuilder UseIngosSwagger(
            this IApplicationBuilder app,
            Action<IngosSwaggerUIOptions> setupAction = null)
        {
            IngosSwaggerUIOptions options;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<IngosSwaggerUIOptions>>().Value;
                setupAction?.Invoke(options);
            }

            return app.UseIngosSwagger(options);
        }
    }
}