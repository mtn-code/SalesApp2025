namespace Verkoop2025Data.Models;

public partial class Artikel
{
    public int ArtikelId { get; set; }

    public string Ean { get; set; } = null!;

    public string Naam { get; set; } = null!;

    public string Beschrijving { get; set; } = null!;

    public decimal Prijs { get; set; }

    public int GewichtInGram { get; set; }

    public int Bestelpeil { get; set; }

    public int Voorraad { get; set; }

    public int MinimumVoorraad { get; set; }

    public int MaximumVoorraad { get; set; }

    public int Levertijd { get; set; }

    public int AantalBesteldLeverancier { get; set; }

    public int MaxAantalInMagazijnPlaats { get; set; }

    public int LeveranciersId { get; set; }

    public virtual ICollection<ArtikelLeveranciersInfolijn> ArtikelLeveranciersInfoLijnen { get; set; } = new List<ArtikelLeveranciersInfolijn>();

    public virtual ICollection<Bestellijn> Bestellijnen { get; set; } = new List<Bestellijn>();

    public virtual ICollection<InkomendeLeveringsLijn> InkomendeLeveringsLijnen { get; set; } = new List<InkomendeLeveringsLijn>();

    public virtual Leverancier Leverancier { get; set; } = null!;

    public virtual ICollection<MagazijnPlaats> MagazijnPlaatsen { get; set; } = new List<MagazijnPlaats>();

    public virtual ICollection<VeelgesteldeVragenArtikel> VeelgesteldeVragenArtikelen { get; set; } = new List<VeelgesteldeVragenArtikel>();

    public virtual ICollection<WishListItem> WishListItems { get; set; } = new List<WishListItem>();

    public virtual ICollection<Categorie> Categorieen { get; set; } = new List<Categorie>();
}
