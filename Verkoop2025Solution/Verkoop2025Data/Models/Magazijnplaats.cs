namespace Verkoop2025Data.Models;

public partial class MagazijnPlaats
{
    public int MagazijnPlaatsId { get; set; }

    public int? ArtikelId { get; set; }

    public string Rij { get; set; } = null!;

    public int Rek { get; set; }

    public int Aantal { get; set; }

    public virtual Artikel? Artikel { get; set; }

    public virtual ICollection<InkomendeLeveringsLijn> InkomendeLeveringsLijnen { get; set; } = new List<InkomendeLeveringsLijn>();
}
