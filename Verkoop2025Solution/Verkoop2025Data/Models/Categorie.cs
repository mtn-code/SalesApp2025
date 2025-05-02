namespace Verkoop2025Data.Models;

public partial class Categorie
{
    public int CategorieId { get; set; }

    public string Naam { get; set; } = null!;

    public int? HoofdCategorieId { get; set; }

    public virtual Categorie? HoofdCategorie { get; set; }

    public virtual ICollection<Categorie> Subcategorieen { get; set; } = new List<Categorie>();

    public virtual ICollection<Artikel> Artikelen { get; set; } = new List<Artikel>();
}
