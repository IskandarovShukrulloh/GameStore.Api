using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString);

// main endpoints
app.MapGet("/", () => "Welcome to the Game Store! Here you can find a variety of games to enjoy!");
app.MapGet("/main", () => "main page, but not ready yet! Please check back later.");
app.MapGamesEndpoints();

app.Run();
    