
using System.ComponentModel.DataAnnotations;

namespace Verkoop2025Web.Models;

    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mailadres is vereist")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    public string? Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is vereist")]
        [DataType(DataType.Password)]
    public string? Password { get; set; }
}
