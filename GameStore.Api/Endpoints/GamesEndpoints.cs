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
        group.MapGet("/", (GameStoreContext dbContext) =>
           dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking());

        /// GET /games/{id}
        group.MapGet("/{id:int}", (int id, GameStoreContext dbContext) =>
        {
            // Check if game with given ID exists
            if (id <= 0)
                return Results.BadRequest("Invalid game ID.");

            Game? game = dbContext.Games.Find(id);

            return game is null
                ? Results.NotFound()
                : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameById); // Name for endpoint to get a game by ID

        /// POST /games
        group.MapPost("/", (CreateGameDTO newGame, GameStoreContext dbContext) =>
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
                dbContext.SaveChanges();
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
        group.MapPut("/{id:int}", (int id, UpdateGameDTO updatedGame, GameStoreContext dbContext) =>
        {
            // 1) ID validation
            if (id <= 0)
                return Results.BadRequest("gameId must be positive");

            // 2) Game existence check
            var existingGame = dbContext.Games.Find(id);
            
            if (existingGame is null)
                return Results.NotFound("game not found");

            // 3) genreId check
            if (updatedGame.GenreId <= 0)
                return Results.BadRequest("genreId must be positive");

            var genreExists = dbContext.Genres.Any(g => g.Id == updatedGame.GenreId);

            if (!genreExists)
                return Results.NotFound("genre not found");

            // 4) Update and save
            dbContext.Entry(existingGame).CurrentValues
                                        .SetValues(updatedGame.ToEntity(id));

            dbContext.SaveChanges();

            // 5) Success response
            return Results.NoContent(); // 204
        });


        /// DELETE /games/{id}
        group.MapDelete("/{id:int}", (int id, GameStoreContext dbContext) =>
        {
            return dbContext.Games.Where(game => game.Id == id).ExecuteDelete() > 0
                ? Results.NoContent() // 204 No Content
                : Results.NotFound(); // 404 Not Found
        });

        return group;
    }
}
    