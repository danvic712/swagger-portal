﻿// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerBuilderExtension.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-23 18:12
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

#if NETSTANDARD2_0
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
#endif

namespace Ingos.AspNetCore.Swagger.DependencyInjection
{
    public static class IngosSwaggerBuilderExtension
    {
        /// <summary>
        /// Register the Swagger middleware with provided options into the HTTP request pipeline
        /// </summary>
        public static IApplicationBuilder UseIngosSwagger(this IApplicationBuilder app, SwaggerOptions options)
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
            if (options.EnableProxy)
                app.UseIngosSwaggerProxies();

            return app;
        }

        private static IApplicationBuilder UseIngosSwaggerProxies(this IApplicationBuilder app)
        {
            return app;
        }

        /// <summary>
        /// Register the Swagger middleware with optional setup action for DI-injected options
        /// </summary>
        public static IApplicationBuilder UseIngosSwagger(
            this IApplicationBuilder app,
            Action<SwaggerOptions> setupAction = null)
        {
            SwaggerOptions options;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<SwaggerOptions>>().Value;
                setupAction?.Invoke(options);
            }

            return app.UseIngosSwagger(options);
        }
    }
}