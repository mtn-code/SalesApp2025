using System.ComponentModel.DataAnnotations;

namespace Verkoop2025Web.Models
{
    public class PasswordChangeViewModel
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
