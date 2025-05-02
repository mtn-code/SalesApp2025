using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mysqlx.Crud;
using Microsoft.EntityFrameworkCore;
using Verkoop2025Data.Models;
using Verkoop2025Data.Repositories;


namespace Verkoop2025Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Klant>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllCustomersAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersByNameAsync(string searchterm)
        {
            return await _customerRepository.GetCustomersByNameAsync(searchterm);
        }

        public async Task<Klant?> GetCustomerDetailsByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerDetailsByIdAsync(customerId);
            return customer;
        }



        public async Task<IEnumerable<Bestelling?>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _customerRepository.GetOrdersByCustomerAsync(customerId);
        }

        public async Task<Adres?> GetDeliveryAddressByCustomerIdAsync(int customerId)
        {
            return await _customerRepository.GetDeliveryAddressByCustomerIdAsync(customerId);
        }

        public async Task<Adres?> GetBillingAdressByCustomerIdAsync(int customerId)
        {
            return await _customerRepository.GetBillingAdressByCustomerIdAsync(customerId);
        }

        public async Task ChangeBillingAdressAsync(Adres newAdress, int klantId)
        {
            Plaats? plaats = await _customerRepository.GetFullPlaatsInfoAsync(newAdress);
            var klant = await _customerRepository.GetCustomerDetailsByIdAsync(klantId);

            if (plaats != null)
            {
                var adressNEw = plaats.Adressen.FirstOrDefault(a => a.Straat.Equals(newAdress.Straat) && a.HuisNummer.Equals(newAdress.HuisNummer) && (string.IsNullOrWhiteSpace(a.Bus) || a.Bus.Equals(newAdress.Bus)));
                if (adressNEw != null)
                {
                    await _customerRepository.DeactivateCurrentBillingAdressAsync(klantId);
                    await _customerRepository.UpdateBillingAdressAsync(newAdress, klantId);
                }
                else
                {
                    await _customerRepository.CreateAdressAsync(newAdress);
                    await _customerRepository.DeactivateCurrentBillingAdressAsync(klantId);
                    await _customerRepository.UpdateBillingAdressAsync(newAdress, klantId);
                }
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public async Task ChangeDeliveryAddressAsync(Adres newAddress, int klantId)
        {
            Plaats? plaats = await _customerRepository.GetFullPlaatsInfoAsync(newAddress);
            var klant = await _customerRepository.GetCustomerDetailsByIdAsync(klantId);

            if (plaats != null)
            {
                var adressNew = plaats.Adressen.FirstOrDefault(a => a.Straat.Equals(newAddress.Straat) && a.HuisNummer.Equals(newAddress.HuisNummer) && (string.IsNullOrWhiteSpace(a.Bus) || a.Bus.Equals(newAddress.Bus)));
                if (adressNew != null)
                {
                    await _customerRepository.DeactivateCurrentDeliveryAddressAsync(klantId);
                    await _customerRepository.UpdateDeliveryAddressAsync(newAddress, klantId);
                }
                else
                {
                    await _customerRepository.CreateAdressAsync(newAddress);
                    await _customerRepository.DeactivateCurrentDeliveryAddressAsync(klantId);
                    await _customerRepository.UpdateDeliveryAddressAsync(newAddress, klantId);
                }
            }
            else
            {
                throw new ApplicationException();
            }
        }
        public async Task<List<SelectListItem>>GetPostcodeSelectListAsync()
        {
            return await _customerRepository.GetPostcodeSelectListAsync();
        }

        public async Task<List<SelectListItem>>GetPlaatsSelectListAsync()
        {
            return await _customerRepository.GetPlaatsSelectListAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersActiefAsync()
        {
            return await _customerRepository.GetCustomersActiefAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersNietActiefAsync()
        {
            return await _customerRepository.GetCustomersNietActiefAsync();

        }
        public async Task<Klant?> ChangeCustomerStatusAsync(int id)
        {
            return await _customerRepository.ChangeCustomerStatusAsync(id);
        }

        public async Task<IEnumerable<Klant>> GetCustomersByPersonTypeAsync(string? personType)
        {
            return await _customerRepository.GetCustomersByPersonTypeAsync(personType);
        }
    }
}