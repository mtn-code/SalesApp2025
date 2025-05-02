namespace Verkoop2025Data.Models;

public partial class UitgaandeLeveringsStatus
{
    public int UitgaandeLeveringsStatusId { get; set; }

    public string Naam { get; set; } = null!;

    public virtual ICollection<UitgaandeLevering> UitgaandeLeveringen { get; set; } = new List<UitgaandeLevering>();
}
