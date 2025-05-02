namespace Verkoop2025Data.Models;

public partial class Rechtspersoon
{
    public int KlantId { get; set; }

    public string Naam { get; set; } = null!;

    public string BtwNummer { get; set; } = null!;

    public virtual ICollection<Contactpersoon> Contactpersonen { get; set; } = new List<Contactpersoon>();

    public virtual Klant Klant { get; set; } = null!;
}
