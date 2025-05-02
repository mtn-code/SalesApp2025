using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Verkoop2025Web.Models;

public class AdressViewModel
{
    [Required]
    public int KlantId { get; set; }
    [Required(ErrorMessage = "Straat is vereist")]
    public string? Straat {  get; set; }
    [Required(ErrorMessage = "Huisnummer is vereist")]
    public string? Huisnummer { get; set; }
    public string? Bus {  get; set; }
    [Required(ErrorMessage = "Plaats is vereist")]
    public string? Plaats { get; set; }
    [Required(ErrorMessage = "Postcode is vereist")]
    public string? Postcode { get; set; }
    public List<SelectListItem> PostcodeSelectList { get; set; } = null!;
    public List<SelectListItem> PlaatsSelectList { get; set; } = null!;
}
