namespace GameStore.Api.DTOs;

public record class GameDetailsDTO
{
    public int Id { get; init; } 
    public string Name { get; init; } = string.Empty;

    public int GenreId { get; init; }

    public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init; }
}
