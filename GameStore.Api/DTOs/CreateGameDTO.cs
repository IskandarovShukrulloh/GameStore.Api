using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.DTOs;

public record class CreateGameDTO
{
    [Required]
    [StringLength(50)]
    public required string Name { get; init; }

    [Required]
    [StringLength(20)]
    public required string Genre { get; init; }

    [Range(0, 696.9)]
    public decimal Price { get; init; }

    [Required]
    public DateOnly ReleaseDate { get; init; }
}
