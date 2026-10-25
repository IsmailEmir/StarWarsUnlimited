namespace StarWarsUnlimited.Models;

/// <summary>Карточка настольной игры Star Wars: Unlimited.</summary>
public class Card
{
    public int Id { get; set; }

    /// <summary>Внешний идентификатор карты в API starwarsunlimited.com.</summary>
    public int ExternalId { get; set; }

    /// <summary>Название карты.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Подзаголовок (вариация имени) карты.</summary>
    public string? Subtitle { get; set; }

    /// <summary>Номер карты внутри сета.</summary>
    public int CardNumber { get; set; }

    /// <summary>Количество карт в сете.</summary>
    public int? CardCount { get; set; }

    /// <summary>Стоимость ресурсов для разыгрывания.</summary>
    public int? Cost { get; set; }

    /// <summary>Прочность (здоровье) карты.</summary>
    public int? Hp { get; set; }

    /// <summary>Сила карты.</summary>
    public int? Power { get; set; }

    /// <summary>Текст способности карты.</summary>
    public string? Text { get; set; }

    /// <summary>Художник карты.</summary>
    public string? Artist { get; set; }

    /// <summary>Серийный номер карты, например 0101005 или IC27010001.</summary>
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>Ссылка на изображение карты.</summary>
    public string? ArtUrl { get; set; }

    /// <summary>Уникальность карты в колоде.</summary>
    public bool IsUnique { get; set; }

    /// <summary>Наличие фольгированного варианта.</summary>
    public bool IsFoil { get; set; }

    /// <summary>Принадлежность к набору Hyperspace.</summary>
    public bool IsHyperspace { get; set; }

    /// <summary>Идентификатор сета.</summary>
    public int SetId { get; set; }

    /// <summary>Идентификатор редкости.</summary>
    public int RarityId { get; set; }

    /// <summary>Идентификатор типа карты.</summary>
    public int TypeId { get; set; }

    /// <summary>Дата создания записи в API.</summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>Дата обновления записи в API.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Навигационное свойство: сет карты (загружается через Include).</summary>
    public Set? Set { get; set; }

    /// <summary>Навигационное свойство: редкость карты.</summary>
    public Rarity? Rarity { get; set; }

    /// <summary>Навигационное свойство: тип карты.</summary>
    public CardType? CardType { get; set; }

    /// <summary>Навигационное свойство: аспекты карты.</summary>
    public List<Aspect> Aspects { get; set; } = new();

    /// <summary>Навигационное свойство: трейты карты.</summary>
    public List<Trait> Traits { get; set; } = new();
}
