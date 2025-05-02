namespace Verkoop2025Data.Models;

public partial class WishListItem
{
    public int WishListItemId { get; set; }

    public int ArtikelId { get; set; }

    public int GebruikersAccountId { get; set; }

    public DateTime AanvraagDatum { get; set; }

    public int Aantal { get; set; }

    public DateTime? EmailGestuurdDatum { get; set; }

    public virtual Artikel Artikel { get; set; } = null!;

    public virtual GebruikersAccount GebruikersAccount { get; set; } = null!;
}
