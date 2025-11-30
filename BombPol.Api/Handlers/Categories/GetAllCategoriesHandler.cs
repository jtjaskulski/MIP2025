using BombPol.Api.Messages.Commands.Categories;
using BombPol.Api.Messages.DTOs;
using BombPol.Api.Messages.Queries;
using BombPol.Data;
using BombPol.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BombPol.Api.Handlers.Categories
{
    public class GetAllCategoriesHandler(BombPolContext context, ILogger<GetAllCategoriesHandler> logger)
        : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Pobieranie wszystkich kategorii bombeczek");
            var result = await context.Category
                .AsNoTracking()
                .FilterOutDeleted()
                .Select(cat => new CategoryDto
                {
                    Id = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description,
                    CreatedAt = cat.CreatedAt,
                    ModifiedAt = cat.ModifiedAt,
                    DeletedAt = cat.DeletedAt
                }).ToListAsync(cancellationToken);


            if (result == null || !result.Any())
            {
                throw new KeyNotFoundException($"Nie mamy bombeczek :-(");
            }

            return result;
        }
    }
}