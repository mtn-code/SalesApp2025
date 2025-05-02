using System;
using Google.Protobuf.WellKnownTypes;
using Verkoop2025Data.Models;
using X.PagedList;

namespace Verkoop2025Web.Models;

public class OrderDetailViewModel
{
    public int BestelId { get; set; }
    public DateTime BestelDatum { get; set; }
    public string? BestellingStatus { get; set; }
    public string? KlantVoornaam { get; set; }
    public string? KlantFamilienaam { get; set; }
    public string? KlantEmail { get; set; }
    public Adres? Facturatieadres { get; set; }
    public Adres? Leveradres { get; set; }
    public string? RechtspersoonBTWNr { get; set; }
    public string? RechtspersoonNaam { get; set; }
    public Betaalwijze? Betaalwijze { get; set; }
    public string? Betalingscode { get; set; }
    public bool? ActiecodeGebruikt { get; set; }
    public IEnumerable<Contactpersoon>? Contactpersonen { get; set; } = new List<Contactpersoon>();
    public string? Status { get; set; }
    public IPagedList<Bestellijn>? Bestellijnen { get; set; }
}