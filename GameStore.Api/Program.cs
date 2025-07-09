using System.Reflection.Metadata.Ecma335;
using GameStore.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// CONSTANTS & main endpoints
const string GetGameByIdName = "GetGame";
app.MapGet("/", () => "Welcome to the Game Store! Here you can find a variety of games to enjoy!");
app.MapGet("/main", () => "main page, but not ready yet! Please check back later.");

// SAMPLE DATA for the games
List<GameDTO> games = [
    new GameDTO
    {
        Id = 1,
        Name = "R.E.P.O",
        Genre = "Horror",
        Price = 6.29m,
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

// GET /games
app.MapGet("/games", () =>  Results.Ok(games));

// GET /games/{id}
app.MapGet("/games/{id:int}", (int id) =>
{
    // If game with given ID exists
    var game = games.Find(game => game.Id == id);
    return game is null
        ? Results.NotFound()
        : Results.Ok(game);
}).WithName(GetGameByIdName); // Name

// POST /games
app.MapPost("/games", (CreateGameDTO newGame) => 
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
    return Results.CreatedAtRoute(GetGameByIdName, new { id = game.Id }, game);
});

// PUT /games/{id}
app.MapPut("games/{id}", (int id, UpdateGameDTO updatedGame) =>
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

// DELETE /games/{id}
app.MapDelete("/games/{id:int}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent(); // 204 No Content
});

app.Run();
