// -----------------------------------------------------------------------
// <copyright file= "BaseController.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2022-07-02 16:42
// Modified by:
// Description:
// -----------------------------------------------------------------------

using Ingos.SwaggerPortal.API.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace Ingos.SwaggerPortal.API.Infrastructure;

/// <summary>
///     Base controller
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RemoteServiceErrorResponse))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(RemoteServiceErrorResponse))]
public abstract class BaseController : AbpController
{
    /// <summary>
    ///     The base controller
    /// </summary>
    protected BaseController()
    {
        LocalizationResource = typeof(IngosResource);
    }
}