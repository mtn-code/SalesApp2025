using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using System.Numerics;
using Verkoop2025Data.Models;
using Verkoop2025Services;
using Verkoop2025Web.Models;
using X.PagedList.Extensions;


namespace Verkoop2025Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;
        private const int pageSize = 5;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var klanten = await _customerService.GetAllCustomersAsync();

            var pagedKlanten = klanten.ToPagedList(page, pageSize);

            var viewModel = new CustomerViewModel
            {
                Klanten = pagedKlanten
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int page = 1)
        {
            try
            {

                var customerDetail = await _customerService.GetCustomerDetailsByIdAsync(id);
                if (customerDetail == null)
                {
                    return View();
                }

                var customerOrders = await _customerService.GetOrdersByCustomerAsync(id);

                var pagedBestellingen = customerOrders.ToPagedList(page, pageSize);

                var customerDetailViewModel = new CustomerDetailViewModel()
                {

                    KlantVoornaam = customerDetail.NatuurlijkePersoon?.Voornaam,
                    KlantFamilienaam = customerDetail.NatuurlijkePersoon?.Familienaam,
                    KlantEmail = customerDetail.NatuurlijkePersoon?.GebruikersAccount.Emailadres,
                    Facturatieadres = customerDetail.FacturatieAdres,
                    Leveradres = customerDetail.LeveringsAdres,
                    RechtspersoonBTWNr = customerDetail.Rechtspersoon?.BtwNummer,
                    RechtspersoonNaam = customerDetail.Rechtspersoon?.Naam,
                    Contactpersonen = customerDetail.Rechtspersoon?.Contactpersonen.OrderBy(v => v.Voornaam),
                    KlantId = id,
                    Bestellingen = pagedBestellingen!,
                    Klant = customerDetail
                };
                return View(customerDetailViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer details: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleCustomerStatus(int id)
        {
            var updatedKlant = await _customerService.ChangeCustomerStatusAsync(id);

            if (updatedKlant == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = updatedKlant.KlantId });
        }



        public async Task<IActionResult> ChangeBillingAdress(int id)
        {

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.CurrentBillingAdress = await _customerService.GetBillingAdressByCustomerIdAsync(id);

            AdressViewModel login = new();
            login.PlaatsSelectList = await _customerService.GetPlaatsSelectListAsync();
            login.PostcodeSelectList = await _customerService.GetPostcodeSelectListAsync();
            login.KlantId = id;
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBillingAdressAsync(AdressViewModel newAdress)
        {
            try
            {
                ModelState.Remove(nameof(newAdress.PostcodeSelectList));
                ModelState.Remove(nameof(newAdress.PlaatsSelectList));
                if (!ModelState.IsValid)
                {
                    ViewBag.CurrentBillingAdress = await _customerService.GetBillingAdressByCustomerIdAsync(newAdress.KlantId);
                    newAdress.PlaatsSelectList = await _customerService.GetPlaatsSelectListAsync();
                    newAdress.PostcodeSelectList = await _customerService.GetPostcodeSelectListAsync();
                    return View("ChangeBillingAdress", newAdress);
                }
                else
                {
                    Adres adres = new Adres()
                    {
                        Straat = newAdress.Straat!,
                        HuisNummer = newAdress.Huisnummer!,
                        Bus = newAdress.Bus,
                        Plaats = new Plaats()
                        {
                            Naam = newAdress.Plaats!,
                            Postcode = newAdress.Postcode!
                        }
                    };
                    await _customerService.ChangeBillingAdressAsync(adres, newAdress.KlantId);
                    return RedirectToAction("Details", new { id = newAdress.KlantId });
                }
            }
            catch (ApplicationException)
            {
                TempData["ErrorMessage"] = "Deze plaats bestaat niet.";
                return RedirectToAction("ChangeBillingAdress", new { id = newAdress.KlantId });
            }
        }
        public async Task<IActionResult> ChangeDeliveryAddress(int id)
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.CurrentDeliveryAddress = await _customerService.GetDeliveryAddressByCustomerIdAsync(id);

            AdressViewModel login = new();
            login.PlaatsSelectList = await _customerService.GetPlaatsSelectListAsync();
            login.PostcodeSelectList = await _customerService.GetPostcodeSelectListAsync();
            login.KlantId = id;
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryAdressAsync(AdressViewModel newAddress)
        {
            try
            {
                ModelState.Remove(nameof(newAddress.PostcodeSelectList));
                ModelState.Remove(nameof(newAddress.PlaatsSelectList));
                if (!ModelState.IsValid)
                {
                    ViewBag.CurrentBillingAdress = await _customerService.GetDeliveryAddressByCustomerIdAsync(newAddress.KlantId);
                    newAddress.PlaatsSelectList = await _customerService.GetPlaatsSelectListAsync();
                    newAddress.PostcodeSelectList = await _customerService.GetPostcodeSelectListAsync();
                    return View("ChangeDeliveryAddress", newAddress);
                }
                else
                {
                    Adres adres = new Adres()
                    {
                        Straat = newAddress.Straat!,
                        HuisNummer = newAddress.Huisnummer!,
                        Bus = newAddress.Bus,
                        Plaats = new Plaats()
                        {
                            Naam = newAddress.Plaats!,
                            Postcode = newAddress.Postcode!
                        }
                    };
                    await _customerService.ChangeDeliveryAddressAsync(adres, newAddress.KlantId);
                    return RedirectToAction("Details", new { id = newAddress.KlantId });
                }
            }
            catch (ApplicationException)
            {
                TempData["ErrorMessage"] = "Deze plaats bestaat niet.";
                return RedirectToAction("ChangeDeliveryAddress", new { id = newAddress.KlantId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers(bool? isActive, int page = 1)
        {
            try
            {
                var klanten = await _customerService.GetAllCustomersAsync();

                // Pas het filter toe als isActive een waarde heeft
                if (isActive.HasValue)
                {
                    if (isActive == true)
                    {
                        klanten = await _customerService.GetCustomersActiefAsync();
                    }
                    else
                    {
                        klanten = await _customerService.GetCustomersNietActiefAsync();
                    }
                }

                var pagedKlanten = klanten.ToPagedList(page, pageSize);

                var viewModel = new CustomerViewModel
                {
                    Klanten = pagedKlanten
                };

                ViewBag.IsActive = isActive;

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers: {ex.Message}");
                return BadRequest("Er is een fout opgetreden bij het ophalen van klanten.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetPersoonstype(bool? isNatuurlijke, int page = 1)
        {
            try
            {
                var klanten = await _customerService.GetAllCustomersAsync();

                // Pas het filter toe als isActive een waarde heeft
                if (isNatuurlijke.HasValue)
                {
                    if (isNatuurlijke == true)
                    {
                        klanten = await _customerService.GetCustomersByPersonTypeAsync("NatuurlijkePersoon");
                    }
                    else
                    {
                        klanten = await _customerService.GetCustomersByPersonTypeAsync("Rechtspersoon");
                    }
                }

                var pagedKlanten = klanten.ToPagedList(page, pageSize);

                var viewModel = new CustomerViewModel
                {
                    Klanten = pagedKlanten
                };

                ViewBag.IsNatuurlijke = isNatuurlijke;

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers: {ex.Message}");
                return BadRequest("Er is een fout opgetreden bij het ophalen van klanten.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchCustomer(string searchterm, int page = 1)
        {
            try
            {
                var customers = await _customerService.GetCustomersByNameAsync(searchterm);
                var pagedCustomers = customers.ToPagedList(page, pageSize);

                var viewModel = new CustomerViewModel
                {
                    Klanten = pagedCustomers
                };

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching klant naam: {ex.Message}");
                return View();
            }
        }
    }
}