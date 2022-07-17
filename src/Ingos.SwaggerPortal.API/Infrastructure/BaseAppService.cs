using Ingos.SwaggerPortal.API.Localization;
using Volo.Abp.Application.Services;

namespace Ingos.SwaggerPortal.API.Infrastructure;

/// <summary>
///     Inherit your application services from this class.
/// </summary>
public abstract class BaseAppService : ApplicationService
{
    /// <summary>
    ///     Base application service
    /// </summary>
    protected BaseAppService()
    {
        LocalizationResource = typeof(IngosResource);
    }
}