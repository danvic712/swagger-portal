// -----------------------------------------------------------------------
// <copyright file= "ProxiesController.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-02 16:40
// Modified by:
// Description:
// -----------------------------------------------------------------------

using Ingos.SwaggerPortal.API.Applications.Contracts;
using Ingos.SwaggerPortal.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Ingos.SwaggerPortal.API.Controllers.v1;

/// <summary>
/// Swagger proxy configuration endpoints
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProxiesController : BaseController
{
    #region Initializes

    private readonly IProxyAppService _proxyAppService;
    
    public ProxiesController(IProxyAppService proxyAppService)
    {
        _proxyAppService = proxyAppService;
    }

    #endregion
}