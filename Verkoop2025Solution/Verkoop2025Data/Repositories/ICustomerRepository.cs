using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkoop2025Data.Models;

namespace Verkoop2025Data.Repositories;
public interface ICustomerRepository
{
    Task<IEnumerable<Klant>> GetCustomersByNameAsync(string searchterm);
    Task<Klant?> GetCustomerDetailsByIdAsync(int id);
    Task<IEnumerable<Klant>> GetAllCustomersAsync();
    Task<IEnumerable<Bestelling?>> GetOrdersByCustomerAsync(int customerId);
    Task<Klant?> ChangeCustomerStatusAsync(int id);
    Task<Adres?> GetBillingAdressByCustomerIdAsync(int customerId);
    Task<Plaats?> GetPlaatsAsync(Plaats plaats);
    Task CreateAdressAsync(Adres adres);
    Task UpdateBillingAdressAsync(Adres adres, int KlantId);
    Task<Plaats?> GetFullPlaatsInfoAsync(Adres adres);
    Task DeactivateCurrentBillingAdressAsync(int klantId);
    Task<List<SelectListItem>> GetPostcodeSelectListAsync();
    Task<List<SelectListItem>> GetPlaatsSelectListAsync();
    Task<IEnumerable<Klant>> GetCustomersActiefAsync();
    Task<IEnumerable<Klant>> GetCustomersNietActiefAsync();
    Task UpdateDeliveryAddressAsync(Adres adres, int klantId);
    Task DeactivateCurrentDeliveryAddressAsync(int klantId);
    Task<Adres?> GetDeliveryAddressByCustomerIdAsync(int customerId);
    Task<IEnumerable<Klant>> GetCustomersByPersonTypeAsync(string? personType);

}


