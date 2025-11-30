using BombPol.Data;
using BombPol.Data.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BombPol.WWW.Controllers.Components
{
    public class ProductsComponent(ILogger<ProductsComponent> logger, BombPolContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Guid? id = null)
        {
            try
            {
                logger.LogInformation("Start ProductsComponent");
                var productsQuery = context.Product.AsNoTracking().FilterOutDeleted();
                if (id != null && id != Guid.Empty)
                {
                    productsQuery = productsQuery.Where(p => p.CategoryId == id);
                }
                return View("ProductsComponent", await productsQuery.ToListAsync());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in ProductsComponent");
                throw;
            }
        }
    }
}