namespace StarWarsUnlimited.Models;

/// <summary>Трейт (признак) карты: Droid, Sith, Vehicle, Bounty Hunter...</summary>
public class Trait
{
    public int Id { get; set; }

    /// <summary>Название трейта.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Описание трейта.</summary>
    public string? Description { get; set; }
}
