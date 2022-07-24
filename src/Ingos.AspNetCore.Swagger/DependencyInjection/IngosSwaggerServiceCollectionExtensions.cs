// -----------------------------------------------------------------------
// <copyright file= "IngosSwaggerServiceCollectionExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-24 16:10
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ingos.AspNetCore.Swagger;
using Ingos.AspNetCore.Swagger.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class IngosSwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddIngosSwagger(this IServiceCollection services,
            Action<IngosSwaggerGenOptions> setupAction = null)
        {
            // Todo: get user configurations
            var options = new IngosSwaggerGenOptions();
            // using (var scope = services.)
            // {
            //     options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<IngosSwaggerGenOptions>>().Value;
            //     setupAction?.Invoke(options);
            // }

            services.AddIngosApiVersion();

            services.AddSwaggerGen(s =>
            {
                // Generate api doc by api version info
                //
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.OpenApiInfo.Version = description.ApiVersion.ToString();
                    s.SwaggerDoc(description.GroupName, options.OpenApiInfo);
                }

                // Show api version in the url which swagger doc generated
                s.DocInclusionPredicate((version, apiDescription) =>
                {
                    // Just show this version's api
                    if (!version.Equals(apiDescription.GroupName))
                        return false;

                    if (apiDescription.RelativePath == null)
                        return false;

                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", apiDescription.GroupName));
                    apiDescription.RelativePath = string.Join("/", values);

                    return true;
                });

                // Let params use the camel naming method
                s.DescribeAllParametersInCamelCase();

                // Remove version param must input in swagger doc
                s.OperationFilter<RemoveVersionFromParameter>();

                // Get project's api description file
                //
                GetSwaggerDocPaths(options.ApiCommentPaths, Path.GetDirectoryName(AppContext.BaseDirectory))
                    .ForEach(x => s.IncludeXmlComments(x, true));
            });

            // Get Swagger generated file paths
            List<string> GetSwaggerDocPaths(IEnumerable<string> paths, string basePath)
            {
                var files = from path in paths
                    let xml = Path.Combine(basePath, path)
                    select xml;

                return files.ToList();
            }

            if (setupAction != null)
                services.ConfigureIngosSwagger(setupAction);

            return services;
        }

        /// <summary>
        ///     Add api version service into services collection
        /// </summary>
        /// <param name="services">Services container</param>
        /// <returns> </returns>
        public static IServiceCollection AddIngosApiVersion(this IServiceCollection services)
        {
            // Add api version support
            services.AddApiVersioning(o =>
            {
                // return api version info in response header
                o.ReportApiVersions = true;

                // default api version
                o.DefaultApiVersion = new ApiVersion(1, 0);

                // when not specifying an api version, select the default version
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            // Config api version info
            services.AddVersionedApiExplorer(option =>
            {
                // Set api version group name format
                option.GroupNameFormat = "'v'VVV";

                // when not specifying an api version, select the default version
                option.AssumeDefaultVersionWhenUnspecified = true;
            });

            return services;
        }

        public static void ConfigureIngosSwagger(this IServiceCollection services,
            Action<IngosSwaggerGenOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}