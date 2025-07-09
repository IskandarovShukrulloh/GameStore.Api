namespace GameStore.Api.DTOs;

public record class CreateGameDTO
{
    public string Name { get; init; } = string.Empty;

    public string Genre { get; init; } = string.Empty;

    public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init; }
}
