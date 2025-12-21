using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Jellyfin.Plugin.Dlna
{
/// <summary>
/// Startup filter to register the DLNA URL middleware.
/// </summary>
public class DlnaStartupFilter : IStartupFilter
{
    /// <summary>
    /// Cette méthode est appelée par le framework ASP.NET Core au démarrage
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            // 1. On injecte notre middleware personnalisé en premier
            builder.UseMiddleware<DlnaUrlMiddleware>();

            // 2. On appelle 'next' pour continuer la configuration 
            // des autres middlewares (ceux de Jellyfin)
            next(builder);
        };
    }
}

}