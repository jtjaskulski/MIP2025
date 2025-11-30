using BombPol.Api.Messages.Commands.Categories;
using BombPol.Api.Messages.DTOs;
using BombPol.Api.Messages.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BombPol.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(IMediator mediator, ILogger<CategoriesController> logger) : Controller
    {
            /// <summary>
            /// Pobiera wszystkie aktywne kategorie
            /// </summary>
            /// <returns>Lista kategorii</returns>
            [HttpGet]
            [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetAll()
            {
                logger.LogInformation("GET /api/category - Pobieranie wszystkich kategorii");
                try
                {

                    var query = new GetAllCategoriesQuery();
                    var result = await mediator.Send(query);

                    return Ok(result);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Pobiera kategorię po ID
            /// </summary>
            /// <param name="id">ID kategorii</param>
            /// <returns>Kategoria</returns>
            [HttpGet("{id}")]
            [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetById(Guid id)
            {
                logger.LogInformation("GET /api/category/{Id} - Pobieranie kategorii", id);

                var query = new GetCategoryByIdQuery(id);
                var result = await mediator.Send(query);

                if (result == null)
                {
                    return NotFound(new { message = $"Kategoria o ID {id} nie została znaleziona" });
                }

                return Ok(result);
            }

            /// <summary>
            /// Tworzy nową kategorię
            /// </summary>
            /// <param name="command">Dane nowej kategorii</param>
            /// <returns>ID utworzonej kategorii</returns>
            [HttpPost]
            [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<IActionResult> Create([FromBody] AddCategoryCommand command)
            {
                logger.LogInformation("POST /api/category - Tworzenie nowej kategorii");

                var categoryId = await mediator.Send(command);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = categoryId },
                    new { id = categoryId, message = "Kategoria została utworzona" }
                );
            }

            /// <summary>
            /// Aktualizuje kategorię
            /// </summary>
            /// <param name="id">ID kategorii</param>
            /// <param name="command">Zaktualizowane dane kategorii</param>
            /// <returns>Status operacji</returns>
            [HttpPut("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> Update(Guid id, [FromBody] EditCategoryCommand command)
            {
                logger.LogInformation("PUT /api/category/{Id} - Aktualizacja kategorii", id);

                if (id != command.Id)
                {
                    return BadRequest(new { message = "ID w URL różni się od ID w body" });
                }

                try
                {
                    await mediator.Send(command);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            /// <summary>
            /// Usuwa kategorię (soft delete)
            /// </summary>
            /// <param name="id">ID kategorii</param>
            /// <returns>Status operacji</returns>
            [HttpDelete("{id}")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> Delete(Guid id)
            {
                logger.LogInformation("DELETE /api/category/{Id} - Usuwanie kategorii", id);

                var command = new DeleteCategoryCommand(id);

                try
                {
                    await mediator.Send(command);
                    return NoContent();
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }