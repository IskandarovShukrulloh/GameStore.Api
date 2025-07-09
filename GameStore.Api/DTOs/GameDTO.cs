namespace GameStore.Api.DTOs;

public record class GameDTO
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public string Genre { get; init; } = string.Empty;

    public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init; }
}
