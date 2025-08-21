using GameStore.Api.Data;
using GameStore.Api.DTOs;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameById = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        /// GET /games
        group.MapGet("/", async (GameStoreContext dbContext) =>
           await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync());

        /// GET /games/{id}
        group.MapGet("/{id:int}", async (int id, GameStoreContext dbContext) =>
        {
            // Check if game with given ID exists
            if (id <= 0)
                return Results.BadRequest("Invalid game ID.");

            Game? game = await dbContext.Games.FindAsync(id);

            return game is null
                ? Results.NotFound()
                : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameById); // Name for endpoint to get a game by ID

        /// POST /games
        group.MapPost("/", async (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            // 1) Validate input
            if (newGame.GenreId <= 0)
                return Results.BadRequest("genreId must be positive");

            // 2) Existing genre check
            var genreExists = dbContext.Genres.Any(g => g.Id == newGame.GenreId);

            if (!genreExists)
                return Results.NotFound("genre not found");

            // 3) Create and save
            var game = newGame.ToEntity();
            dbContext.Games.Add(game);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle save failure
                return Results.BadRequest("failed to save game");
            }

            // 4) 201 + Location + DTO
            return Results.CreatedAtRoute(
                GetGameById,
                new { id = game.Id },
                game.ToGameDetailsDto()
            );
        });


        /// PUT /games/{id}
        group.MapPut("/{id:int}", async (int id, UpdateGameDTO updatedGame, GameStoreContext dbContext) =>
        {
            // 1) ID validation
            if (id <= 0)
                return Results.BadRequest("gameId must be positive");

            // 2) Game existence check
            var existingGame = await dbContext.Games.FindAsync(id);
            
            if (existingGame is null)
                return Results.NotFound("game not found");

            // 3) genreId check
            if (updatedGame.GenreId <= 0)
                return Results.BadRequest("genreId must be positive");

            var genreExists = await dbContext.Genres.AnyAsync(g => g.Id == updatedGame.GenreId);

            if (!genreExists)
                return Results.NotFound("genre not found");

            // 4) Update and save
            dbContext.Entry(existingGame).CurrentValues
                                        .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            // 5) Success response
            return Results.NoContent(); // 204
        });


        /// DELETE /games/{id}
        group.MapDelete("/{id:int}", async (int id, GameStoreContext dbContext) =>
        {
            return await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync() > 0
                ? Results.NoContent() // 204 No Content
                : Results.NotFound(); // 404 Not Found
        });

        return group;
    }
}
    