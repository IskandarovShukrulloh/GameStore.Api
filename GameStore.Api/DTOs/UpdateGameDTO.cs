using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record class UpdateGameDTO
{
    [Required][StringLength(50)] public string Name { get; init; } = string.Empty;

    [Required][StringLength(20)] public string Genre { get; init; } = string.Empty;

    [Range(0, 696.9)] public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init; }
}
