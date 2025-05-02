namespace Verkoop2025Data.Models;

public partial class PersoneelslidAccount
{
    public int PersoneelslidAccountId { get; set; }

    public string Emailadres { get; set; } = null!;

    public string Paswoord { get; set; } = null!;

    public bool Disabled { get; set; }

    public virtual ICollection<ChatgesprekLijn> ChatgesprekLijnen { get; set; } = new List<ChatgesprekLijn>();

    public virtual Personeelslid? Personeelslid { get; set; }
}
