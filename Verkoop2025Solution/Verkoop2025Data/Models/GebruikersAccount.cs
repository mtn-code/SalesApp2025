namespace Verkoop2025Data.Models;

public partial class GebruikersAccount
{
    public int GebruikersAccountId { get; set; }

    public string Emailadres { get; set; } = null!;

    public string Paswoord { get; set; } = null!;

    public bool Disabled { get; set; }

    public virtual ICollection<Chatgesprek> Chatgesprekken { get; set; } = new List<Chatgesprek>();

    public virtual ICollection<ChatgesprekLijn> ChatgesprekLijnen { get; set; } = new List<ChatgesprekLijn>();

    public virtual ICollection<Contactpersoon> Contactpersonen { get; set; } = new List<Contactpersoon>();

    public virtual ICollection<NatuurlijkePersoon> NatuurlijkePersonen { get; set; } = new List<NatuurlijkePersoon>();

    public virtual ICollection<WishListItem> WishListItems { get; set; } = new List<WishListItem>();
}
