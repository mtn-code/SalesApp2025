using System;
using System.ComponentModel.DataAnnotations;
using Verkoop2025Data.Models;
using X.PagedList;

namespace Verkoop2025Web.Models;

public class OrderViewModel
{
    public IPagedList<Bestelling>? Bestellingen { get; set; }
}
