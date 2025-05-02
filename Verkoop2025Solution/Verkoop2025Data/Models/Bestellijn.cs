namespace Verkoop2025Data.Models;

public partial class Bestellijn
{
    public int BestellijnId { get; set; }

    public int BestelId { get; set; }

    public int ArtikelId { get; set; }

    public int AantalBesteld { get; set; }

    public int AantalGeannuleerd { get; set; }

    public virtual Artikel Artikel { get; set; } = null!;

    public virtual Bestelling Bestelling { get; set; } = null!;

    public virtual ICollection<KlantenReview> KlantenReviews { get; set; } = new List<KlantenReview>();
}
