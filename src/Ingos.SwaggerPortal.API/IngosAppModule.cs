// -----------------------------------------------------------------------
// <copyright file= "IngosApiModule.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-06-18 17:11
// Modified by:
// Description:
// -----------------------------------------------------------------------

using Ingos.SwaggerPortal.API.Localization;
using Ingos.SwaggerPortal.API.Utils;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Validation.Localization;

namespace Ingos.SwaggerPortal.API;

[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAutoMapperModule)
)]
public class IngosAppModule : AbpModule
{
    private const string CorsPolicyName = "Ingos";

    #region Services

    /// <summary>
    ///     Pre configure before inject services into service collection
    /// </summary>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            // Set dynamic api router with api version info
            options.ConventionalControllers.Create(typeof(IngosAppModule).Assembly,
                opts => { opts.RootPath = "v{version:apiVersion}"; });
        });
    }

    /// <summary>
    ///     Configure application services
    /// </summary>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddHttpClient();

        ConfigureHealthChecks(context);
        ConfigureAntiForgeryTokens();
        ConfigureConventionalControllers(context);
        ConfigureLocalization();
        ConfigureCors(context, configuration);
        ConfigureSwagger(context);
        ConfigureYarpProxy(context, configuration);
    }

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseAbpRequestLocalization();

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(CorsPolicyName);
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseHealthChecks("/health");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Ingos API";

            // Display latest api version by default
            //
            var provider = context.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
            var apiVersionList = provider.ApiVersionDescriptions
                .Select(i => $"v{i.ApiVersion.MajorVersion}")
                .Distinct().Reverse();
            foreach (var apiVersion in apiVersionList)
                options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json",
                    $"Ingos Swagger Portal API {apiVersion?.ToUpperInvariant()}");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();

        app.UseEndpoints(endpoints => { endpoints.MapReverseProxy(); });
    }

    #endregion Services

    #region Methods

    private static void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddHealthChecks();
    }

    private void ConfigureAntiForgeryTokens()
    {
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.FromDays(365);
            options.AutoValidateIgnoredHttpMethods.Add("POST");
        });
    }

    private void ConfigureConventionalControllers(ServiceConfigurationContext context)
    {
        Configure<AbpAspNetCoreMvcOptions>(options => { context.Services.ExecutePreConfiguredActions(options); });

        // Use lowercase routing and lowercase query string
        context.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        context.Services.AddAbpApiVersioning(options =>
        {
            options.ReportApiVersions = true;

            options.AssumeDefaultVersionWhenUnspecified = true;

            options.DefaultApiVersion = new ApiVersion(1, 0);

            options.ApiVersionReader = new UrlSegmentApiVersionReader();

            var mvcOptions = context.Services.ExecutePreConfiguredActions<AbpAspNetCoreMvcOptions>();
            options.ConfigureAbp(mvcOptions);
        });

        context.Services.AddVersionedApiExplorer(option =>
        {
            option.GroupNameFormat = "'v'VVV";

            option.AssumeDefaultVersionWhenUnspecified = true;
        });
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddSwaggerGen(
            options =>
            {
                // Get application api version info
                var provider = context.Services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                // Generate swagger by api major version
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Contact = new OpenApiContact
                        {
                            Name = "Danvic Wang",
                            Email = "danvic.wang@outlook.com",
                            Url = new Uri("https://yuiter.com")
                        },
                        Description = "Ingos Swagger Portal API",
                        Title = "Ingos Swagger Portal API",
                        Version = $"v{description.ApiVersion.MajorVersion}"
                    });

                options.DocInclusionPredicate((docName, description) =>
                {
                    // Get api major version
                    var apiVersion = $"v{description.GetApiVersion().MajorVersion}";

                    if (!docName.Equals(apiVersion))
                        return false;

                    // Replace router parameter
                    var values = description.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", apiVersion));

                    description.RelativePath = string.Join("/", values);

                    return true;
                });

                // Let params use the camel naming method
                options.DescribeAllParametersInCamelCase();

                // Remove version parameter info input in swagger page
                options.OperationFilter<RemoveVersionFromParameter>();

                // Inject api and dto comments
                //
                var paths = new List<string>
                {
                    @"wwwroot/Ingos.SwaggerPortal.API.xml"
                };
                GetApiDocPaths(paths, Path.GetDirectoryName(AppContext.BaseDirectory))
                    .ForEach(x => options.IncludeXmlComments(x, true));
            });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IngosResource>("zh-Hans")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Ingos");

            options.DefaultResourceType = typeof(IngosResource);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<IngosResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );

            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }

    private static void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    /// <summary>
    ///     Get the api description doc path
    /// </summary>
    /// <param name="paths">The xml file path</param>
    /// <param name="basePath">The site's base running files path</param>
    /// <returns></returns>
    private static List<string> GetApiDocPaths(IEnumerable<string> paths, string basePath)
    {
        var files = from path in paths
            let xml = Path.Combine(basePath, path)
            select xml;

        return files.ToList();
    }

    private static void ConfigureYarpProxy(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // Add the reverse proxy to capability to the server
        var proxyBuilder = context.Services.AddReverseProxy();
        // Initialize the reverse proxy from the "ReverseProxy" section of configuration
        proxyBuilder.LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }

    #endregion Methods
}