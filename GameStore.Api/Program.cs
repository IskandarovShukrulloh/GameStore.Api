using GameStore.Api.DTOs;
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

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
        Name = "Manor Lords",
        Genre = "Strategy",
        Price = 19.99m,
        ReleaseDate = new DateOnly(2024, 4, 26)
    },
    new GameDTO
    {
        Id = 3,
        Name = "Cities: Skylines II",
        Genre = "Simulation",
        Price = 33.99m,
        ReleaseDate = new DateOnly(2023, 10, 24)
    },
    new GameDTO
    {
        Id = 4,
        Name = "Counter-Strike 2",
        Genre = "Shooter",
        Price = 15.00m, // But free to play
        ReleaseDate = new DateOnly(2023, 9, 27)
    },
    new GameDTO
    {
        Id = 5,
        Name = "Assassin's Creed Origins",
        Genre = "Action",
        Price = 29.99m,
        ReleaseDate = new DateOnly(2017, 10, 27)
    },  
];

app.MapGet("/", () => "Welcome to the Game Store! Here you can find a variety of games to enjoy.");
app.MapGet("/main", () => "main page, but not ready yet! Please check back later.");

// GET /games
// Returns a list of games
app.MapGet("/games", () =>  Results.Ok(games));

app.Run();
