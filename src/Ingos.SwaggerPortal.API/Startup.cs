// -----------------------------------------------------------------------
// <copyright file= "Startup.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-06-18 17:18
// Modified by:
// Description: Application start up configuration
// -----------------------------------------------------------------------

namespace Ingos.SwaggerPortal.API;

/// <summary>
///     Application start up configuration
/// </summary>
public class Startup
{
    /// <summary>
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<IngosAppModule>();
    }

    /// <summary>
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.InitializeApplication();
    }
}