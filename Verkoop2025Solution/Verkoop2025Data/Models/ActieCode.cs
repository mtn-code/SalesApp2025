namespace Verkoop2025Data.Models;

public partial class Actiecode
{
    public int ActiecodeId { get; set; }

    public string Naam { get; set; } = null!;

    public DateTime GeldigVanDatum { get; set; }

    public DateTime GeldigTotDatum { get; set; }

    public bool IsEenmalig { get; set; }
}
