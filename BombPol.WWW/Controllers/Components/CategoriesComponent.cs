using BombPol.Data;
using BombPol.Data.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BombPol.WWW.Controllers.Components
{
    public class CategoriesComponent(ILogger<CategoriesComponent> logger, BombPolContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start CategoriesComponent");
                return View("CategoriesComponent", await context.Category.Include(cat => cat.Products).AsNoTracking().FilterOutDeleted().ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in CategoriesComponent");
                throw;
            }
        }
    }
}