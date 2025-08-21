using GameStore.Api.DTOs;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDTO game)
    {
        return new Game()
        {
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static Game ToEntity(this UpdateGameDTO game, int id)
    {
        return new Game()
        {
            Id = id,
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static GameSummaryDTO ToGameSummaryDto(this Game game)
    {
        return new GameSummaryDTO()
        {
            Id = game.Id,
            Name = game.Name,
            Genre = game.Genre?.Name ?? string.Empty,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

        public static GameDetailsDTO ToGameDetailsDto(this Game game)
    {
        return new GameDetailsDTO()
        {
            Id = game.Id,
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }
}