using Verkoop2025Data.Models;
using X.PagedList;

namespace Verkoop2025Web.Models;

public class CustomerViewModel
{
    public IPagedList<Klant>? Klanten { get; set; }
}
