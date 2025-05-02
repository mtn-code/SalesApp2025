namespace Verkoop2025Data.Models;

public partial class Klant
{
    public int KlantId { get; set; }

    public int FacturatieAdresId { get; set; }

    public int LeveringsAdresId { get; set; }

    public virtual ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();

    public virtual Adres FacturatieAdres { get; set; } = null!;

    public virtual Adres LeveringsAdres { get; set; } = null!;

    public virtual NatuurlijkePersoon? NatuurlijkePersoon { get; set; }

    public virtual Rechtspersoon? Rechtspersoon { get; set; }

    public virtual ICollection<UitgaandeLevering> UitgaandeLeveringen { get; set; } = new List<UitgaandeLevering>();
}
