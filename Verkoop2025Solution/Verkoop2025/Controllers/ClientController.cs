using Microsoft.AspNetCore.Mvc;
using Verkoop2025Web.Models;
using Verkoop2025Services;
using Verkoop2025Data.Models;
using Verkoop2025Web.Filters;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Mysqlx.Crud;
using X.PagedList.Extensions;
using System.Text.RegularExpressions;


namespace Verkoop2025Web.Controllers;
public class ClientController : Controller
{
    private readonly AccountService accountService;

    public ClientController(AccountService accountService)
    {
        this.accountService = accountService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [SkipFilter]
    public IActionResult Login()
    {
        ViewBag.ErrorMessage = TempData["ErrorMessage"];
        LoginViewModel login = new();
        return View(login);
    }


    public async Task<IActionResult> Authenticate(LoginViewModel login)
    {
        if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
        {
            TempData["ErrorMessage"] = "Vul beide velden in";
            return RedirectToAction("Login", "Client");
        }
        else
        {
            Personeelslid? user = await accountService.AuthenticateUserAsync(login.Email, login.Password);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Deze gebruiker heeft geen toegang";
            }
            else
            {
                HttpContext.Session.SetString("Username", $"{user.Familienaam} {user.Voornaam}");
                HttpContext.Session.SetString("id", user.PersoneelslidAccountId.ToString());
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Client");
        }
    }
    public async Task LogOut()
    {
        await Task.Run(() =>
        {
            HttpContext.Session.Clear();
        });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
    {
        ViewBag.OldPasswordError = null;
        ViewBag.ConfirmPasswordError = null;
        ViewBag.NewPasswordError = null;
        try
        {
            var accountId = HttpContext.Session.GetString("id");

            if (!await accountService.VerifyOldPassword(int.Parse(accountId!), oldPassword))
            {
                ViewBag.OldPasswordError = "Geen geldig oud wachtwoord.";
                return View("Index");
            }
            if (oldPassword == newPassword)
            {
                ViewBag.NewPasswordError = "Nieuw wachtwoord mag niet hetzelfde zijn als oud wachtwoord.";
                return base.View("Index");
            }
            if (newPassword != confirmNewPassword)
            {
                ViewBag.ConfirmPasswordError = "Nieuwe wachtwoorden komen niet overeen.";
                return base.View("Index");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new Exception("Wachtwoord mag niet leeg zijn.");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(newPassword))
            {
                ViewBag.NewPasswordError = "Wachtwoord moet minstens één kleine letter bevatten.";
                return base.View("Index");
            }
            else if (!hasUpperChar.IsMatch(newPassword))
            {
                ViewBag.NewPasswordError = "Wachtwoord moet minstens één grote letter bevatten.";
                return base.View("Index");
            }
            else if (!hasMiniMaxChars.IsMatch(newPassword))
            {
                ViewBag.NewPasswordError = "Wachtwoord moet minstens 8 tekens bevatten.";
                return base.View("Index");
            }
            else if (!hasNumber.IsMatch(newPassword))
            {
                ViewBag.NewPasswordError = "Wachtwoord moet minstens één cijfer bevatten.";
                return base.View("Index");
            }

            else if (!hasSymbols.IsMatch(newPassword))
            {
                ViewBag.NewPasswordError = "Wachtwoord moet minstens één speciaal teken bevatten.";
                return base.View("Index");
            }
            else

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                ViewBag.OldPasswordError = "Vul alle velden in.";
                return base.View("Index");
            }

            var changenewPassword = await accountService.ChangePassword(int.Parse(accountId!), newPassword);
            if (changenewPassword != null)
            {
                TempData["SuccessMessage"] = "Wachtwoord succesvol gewijzigd.";
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.OldPasswordError = ex.Message;
            return View("Index");
        }
    }
}


