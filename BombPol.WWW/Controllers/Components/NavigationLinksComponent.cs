using BombPol.Data;
using BombPol.Data.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BombPol.WWW.Controllers.Components
{
    public class NavigationLinksComponent(ILogger<NavigationLinksComponent> logger, BombPolContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start NavigationLinksComponent");
                return View("NavigationLinksComponent", await context.NavigationLinks.AsNoTracking().FilterOutDeleted().ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in NavigationLinksComponent");
                throw;
            }
        }
    }
}