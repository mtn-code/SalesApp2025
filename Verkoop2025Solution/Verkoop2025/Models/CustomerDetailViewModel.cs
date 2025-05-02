using Verkoop2025Data.Models;
using X.PagedList;
namespace Verkoop2025Web.Models;

public class CustomerDetailViewModel
{
    public int KlantId { get; set; }
    public Adres? Facturatieadres { get; set; }
    public Adres? Leveradres { get; set; }
    public string? KlantVoornaam { get; set; }
    public string? KlantFamilienaam { get; set; }
    public string? KlantEmail { get; set; }
    public string? RechtspersoonNaam { get; set; }
    public string? RechtspersoonBTWNr { get; set; }
    public IEnumerable<Contactpersoon>? Contactpersonen { get; set; } = new List<Contactpersoon>();

    public IPagedList<Bestelling>? Bestellingen { get; set; }
    public Klant? Klant { get; set; }
    public bool Disabled { get; set; }

}


