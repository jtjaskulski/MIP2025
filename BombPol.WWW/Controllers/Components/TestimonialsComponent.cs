using BombPol.Data;
using Microsoft.AspNetCore.Mvc;

namespace BombPol.WWW.Controllers.Components
{
    public class TestimonialsComponent(ILogger<TestimonialsComponent> logger, BombPolContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                logger.LogInformation("Start TestimonialsComponent");
                return View("TestimonialsComponent");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in TestimonialsComponent");
                throw;
            }
        }
    }
}