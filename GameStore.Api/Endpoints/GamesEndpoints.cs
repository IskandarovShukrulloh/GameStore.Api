using GameStore.Api.DTOs;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameById = "GetGame";

    // SAMPLE DATA for the games
    private static readonly List<GameDTO> games = [
        new GameDTO
    {
        Id = 1,
        Name = "The Legend of Zelda: Tears of the Kingdom",
        Genre = "Adventure",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2025, 2, 26)
    },
    new GameDTO
    {
        Id = 2,
        Name = "Cities: Skylines II",
        Genre = "Simulation",
        Price = 33.99m,
        ReleaseDate = new DateOnly(2023, 10, 24)
    },
    new GameDTO
    {
        Id = 3,
        Name = "Assassin's Creed Origins",
        Genre = "Action",
        Price = 29.99m,
        ReleaseDate = new DateOnly(2017, 10, 27)
    },
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        /// GET /games
        group.MapGet("/", () => Results.Ok(games));

        /// GET /games/{id}
        group.MapGet("/{id:int}", (int id) =>
    {
        // Check if game with given ID exists
        if (id <= 0)
            return Results.BadRequest("Invalid game ID.");

        var game = games.Find(game => game.Id == id);

        return game is null
            ? Results.NotFound()
            : Results.Ok(game);
    }).WithName(GetGameById); // Name for endpoint to get a game by ID

        /// POST /games
        group.MapPost("/", (CreateGameDTO newGame) =>
        {
            // New game with the provided data
            var game = new GameDTO
            {
                Id = games.Count + 1, // ID generation
                Name = newGame.Name,
                Genre = newGame.Genre,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            // Add new game to list
            games.Add(game);

            // Return new game with 201 status code
            return Results.CreatedAtRoute(GetGameById, new { id = game.Id }, game);
        });

        /// PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
        {
            // Check if game with given ID exists
            int index = games.FindIndex(game => game.Id == id);

            if (index == -1)
                return Results.NotFound(); // 404 Not Found

            games[index] = new GameDTO
            {
                Id = id, // Keep the same ID
                Name = updatedGame.Name,
                Genre = updatedGame.Genre,
                Price = updatedGame.Price,
                ReleaseDate = updatedGame.ReleaseDate
            };

            return Results.NoContent(); // 204 No Content
        });

        /// DELETE /games/{id}
        group.MapDelete("/{id:int}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            if (games.Count == 0)
                return Results.NotFound(); // 404 Not Found if no games left

            return Results.NoContent(); // 204 No Content
        });

        return group;
    }
}