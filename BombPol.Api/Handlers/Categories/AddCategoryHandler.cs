using BombPol.Api.Messages.Commands.Categories;
using BombPol.Data;
using BombPol.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BombPol.Api.Handlers.Categories
{
    public class AddCategoryHandler(BombPolContext context, ILogger<AddCategoryHandler> logger) 
        : IRequestHandler<AddCategoryCommand,Guid>
    { 
        public async Task<Guid> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Tworzenie nowej kategorii bombeczek: {Name}", request.Name);
             
            var category = new Category(Guid.NewGuid(),request.Name, request.Description ?? string.Empty);
            
            await context.AddAsync(category);
            await context.Category.Include(cat => cat.Products).LoadAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Utworzono kategorię ID: {Id}", category.Id);

            return category.Id;
        }
    }
}