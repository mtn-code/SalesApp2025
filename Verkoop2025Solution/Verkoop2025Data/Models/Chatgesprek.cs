namespace Verkoop2025Data.Models;

public partial class Chatgesprek
{
    public int ChatgesprekId { get; set; }

    public int GebruikersAccountId { get; set; }

    public virtual ICollection<ChatgesprekLijn> ChatgesprekLijnen { get; set; } = new List<ChatgesprekLijn>();

    public virtual GebruikersAccount GebruikersAccount { get; set; } = null!;
}
