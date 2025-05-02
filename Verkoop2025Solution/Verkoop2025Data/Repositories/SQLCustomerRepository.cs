using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Verkoop2025Data.Models;

namespace Verkoop2025Data.Repositories;

public class SQLCustomerRepository : ICustomerRepository
{
    private readonly PrulariaComContext _dbcontext;
    public SQLCustomerRepository(PrulariaComContext dbcontext)
    {
        _dbcontext = dbcontext;
    }


    public async Task<IEnumerable<Klant>> GetCustomersByNameAsync(string searchterm)
        {
            return await _dbcontext.Klanten
                .Include(k => k.FacturatieAdres)
                .Include(k => k.LeveringsAdres)
                .Include(v => v!.NatuurlijkePersoon)
                    .ThenInclude(g => g!.GebruikersAccount) 
                .Include(n => n!.Rechtspersoon)
                .Include(k => k.Bestellingen)
                .Where(n => n.NatuurlijkePersoon!.Voornaam.Contains(searchterm) ||
                            n.NatuurlijkePersoon!.Familienaam.Contains(searchterm) ||
                            n.Rechtspersoon!.Naam.Contains(searchterm))
                .ToListAsync();

        }

        public async Task<Klant?> GetCustomerDetailsByIdAsync(int id)
        {
            var customer = await _dbcontext.Klanten
                .Include(k => k.NatuurlijkePersoon)
                    .ThenInclude(np => np!.GebruikersAccount)
                .Include(k => k.Rechtspersoon)
                    .ThenInclude(cp => cp!.Contactpersonen)
                    .ThenInclude(ga => ga.GebruikersAccount)
                .Include(k => k.FacturatieAdres)
                    .ThenInclude(p => p.Plaats)
                .Include(k => k.LeveringsAdres)
                    .ThenInclude(p => p.Plaats)
                .Include(k => k.Bestellingen)
                    .ThenInclude(bs => bs.BestellingsStatus)
                    .Include(k => k.Bestellingen)
                    .ThenInclude(bl => bl.Bestellijnen)
                .FirstOrDefaultAsync(k => k.KlantId == id);
            return customer;
        }

        public async Task<IEnumerable<Klant>> GetAllCustomersAsync()
        {
            return await _dbcontext.Klanten
                        .Include(k => k.FacturatieAdres)
                        .Include(k => k.LeveringsAdres)
                        .Include(k => k.NatuurlijkePersoon)
                            .ThenInclude(g => g!.GebruikersAccount)
                        .Include(k => k.Rechtspersoon)
                        .Include(k => k.Bestellingen)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Bestelling?>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _dbcontext.Bestellingen
                        .Include(b => b.BestellingsStatus)
                        .Include(b => b.Bestellijnen)
                            .ThenInclude(a => a.Artikel)
                        .Where(k => k.KlantId == customerId)
                        .OrderByDescending(b => b.Besteldatum)
                        .ToListAsync();
        }

        public async Task<Adres?> GetBillingAdressByCustomerIdAsync(int customerId)
        {
            return await _dbcontext.Adressen
                .Include(a => a.Plaats)
                .Where(a => a.KlantenFacturatieAdres != null && a.KlantenFacturatieAdres.Any(k => k.KlantId == customerId))
                .FirstOrDefaultAsync();
        }
        public async Task<Adres?> GetDeliveryAddressByCustomerIdAsync(int customerId)
        {
            return await _dbcontext.Adressen
                .Include(a => a.Plaats)
                .Where(a => a.KlantenLeveringsAdres != null && a.KlantenLeveringsAdres.Any(k => k.KlantId == customerId))
                .FirstOrDefaultAsync();
        }
        public async Task<Plaats?> GetPlaatsAsync(Plaats plaats)
        {
            return await _dbcontext.Plaatsen
                .Where(p => p.Naam == plaats.Naam &&
                           p.Postcode == plaats.Postcode)
                .FirstOrDefaultAsync();
        }

        public async Task<Plaats?> GetFullPlaatsInfoAsync(Adres adres)
        {
            return await _dbcontext.Plaatsen
                .Include(p => p.Adressen)
                .Where(p => p.Naam == adres.Plaats.Naam &&
                            p.Postcode == adres.Plaats.Postcode)
                .FirstOrDefaultAsync();
        }

        public async Task CreatePlaatsAsync(Plaats plaats)
        {
            _dbcontext.Plaatsen.Add(plaats);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task CreateAdressAsync(Adres adres)
        {
            var plaats = await GetPlaatsAsync(adres.Plaats);
            adres.Plaats = plaats!;
            adres.PlaatsId = plaats!.PlaatsId;
            adres.Bus = (adres.Bus == null ? string.Empty : adres.Bus);
            _dbcontext.Adressen.Attach(adres);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateBillingAdressAsync(Adres adres, int klantId)
        {
            var klant = await _dbcontext.Klanten
        .Where(k => k.KlantId == klantId)
        .FirstOrDefaultAsync();

            if (klant == null)
                throw new InvalidOperationException("Klant niet gevonden.");

            var plaats = await GetPlaatsAsync(adres.Plaats);
            var bestaandAdres = await _dbcontext.Adressen
                .Where(a => a.Straat == adres.Straat &&
                            a.HuisNummer == adres.HuisNummer &&
                            a.Bus == (adres.Bus == null ? string.Empty : adres.Bus) &&
                            a.PlaatsId == plaats!.PlaatsId)
                .FirstOrDefaultAsync();

            if (bestaandAdres == null)
            {
                bestaandAdres = new Adres
                {
                    Straat = adres.Straat,
                    HuisNummer = adres.HuisNummer,
                    Bus = (adres.Bus == null ? string.Empty : adres.Bus),
                    Actief = true,
                    PlaatsId = plaats!.PlaatsId
                };

                _dbcontext.Adressen.Add(bestaandAdres);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                bestaandAdres.Actief = true;
                _dbcontext.Adressen.Update(bestaandAdres);
            }
            klant.FacturatieAdresId = bestaandAdres.AdresId;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeactivateCurrentBillingAdressAsync(int klantId)
        {
            var currentBillingAdress = await GetBillingAdressByCustomerIdAsync(klantId);
            if (currentBillingAdress != null)
            {
                currentBillingAdress.Actief = false;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task DeactivateCurrentDeliveryAddressAsync(int klantId)
        {
            var currentDeliveryAdress = await GetDeliveryAddressByCustomerIdAsync(klantId);
            if (currentDeliveryAdress != null)
            {
                currentDeliveryAdress.Actief = false;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<List<SelectListItem>> GetPostcodeSelectListAsync()
        {
            return await _dbcontext.Plaatsen
                .Select(p => new SelectListItem
                {
                    Value = p.Postcode,
                    Text = p.Postcode
                })
                .Distinct()
                .OrderBy(p => p.Value)
                .ToListAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersActiefAsync()
        {
            return await _dbcontext.Klanten
                        .Include(k => k.FacturatieAdres)
                        .Include(k => k.LeveringsAdres)
                        .Include(k => k.NatuurlijkePersoon)
                            .ThenInclude(g => g!.GebruikersAccount)
                        .Include(k => k.Bestellingen)
                        .Where(k => k!.NatuurlijkePersoon != null &&
                                    k!.NatuurlijkePersoon!.GebruikersAccount != null &&
                                    k.NatuurlijkePersoon.GebruikersAccount.Disabled == false)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersNietActiefAsync()
        {
            return await _dbcontext.Klanten
                        .Include(k => k.FacturatieAdres)
                        .Include(k => k.LeveringsAdres)
                        .Include(k => k.NatuurlijkePersoon)
                            .ThenInclude(g => g!.GebruikersAccount)
                        .Include(k => k.Bestellingen)
                        .Where(k => k.NatuurlijkePersoon != null &&
                                    k.NatuurlijkePersoon.GebruikersAccount.Disabled == true)
                        .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetPlaatsSelectListAsync()
        {
            return await _dbcontext.Plaatsen
                .Select(p => new SelectListItem
                {
                    Value = p.Naam,
                    Text = p.Naam
                })
                .ToListAsync();
        }

        public async Task UpdateDeliveryAddressAsync(Adres adres, int klantId)
        {
            var klant = await _dbcontext.Klanten
         .Where(k => k.KlantId == klantId)
         .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Klant niet gevonden.");
            var plaats = await GetPlaatsAsync(adres.Plaats);
            var bestaandAdres = await _dbcontext.Adressen
                .Where(a => a.Straat == adres.Straat &&
                            a.HuisNummer == adres.HuisNummer &&
                            a.Bus == (adres.Bus == null ? string.Empty : adres.Bus) &&
                            a.PlaatsId == plaats!.PlaatsId)
                .FirstOrDefaultAsync();

            if (bestaandAdres == null)
            {
                bestaandAdres = new Adres
                {
                    Straat = adres.Straat,
                    HuisNummer = adres.HuisNummer,
                    Bus = adres.Bus ?? string.Empty,
                    Actief = true,
                    PlaatsId = plaats!.PlaatsId
                };

                _dbcontext.Adressen.Add(bestaandAdres);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                bestaandAdres.Actief = true;
                _dbcontext.Adressen.Update(bestaandAdres);
            }
            klant.LeveringsAdresId = bestaandAdres.AdresId;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Klant>> GetCustomersByPersonTypeAsync(string? personType)
        {
            var filteredCustomers = new List<Klant>();
            if (personType == "NatuurlijkePersoon")
            {
                filteredCustomers = await _dbcontext.Klanten
                                    .Include(k => k.FacturatieAdres)
                                    .Include(k => k.LeveringsAdres)
                                    .Include(k => k.NatuurlijkePersoon)
                                        .ThenInclude(g => g!.GebruikersAccount)
                                    .Include(k => k.Bestellingen)
                                    .Where(k => k.NatuurlijkePersoon != null)
                                    .ToListAsync();
            }
            else
            {
                filteredCustomers = await _dbcontext.Klanten
                                    .Include(k => k.FacturatieAdres)
                                    .Include(k => k.LeveringsAdres)
                                    .Include(k => k.Rechtspersoon)
                                    .Include(k => k.Bestellingen)
                                    .Where(k => k.Rechtspersoon != null)
                                    .ToListAsync();
            }
            return filteredCustomers;
        }



    public async Task<Klant?> ChangeCustomerStatusAsync(int id)
    {
        var klant = await _dbcontext.Klanten
            .Include(k => k.NatuurlijkePersoon)
            .ThenInclude(np => np!.GebruikersAccount)
            .FirstOrDefaultAsync(k => k.NatuurlijkePersoon != null && k.NatuurlijkePersoon.GebruikersAccount != null && k.NatuurlijkePersoon.GebruikersAccount.GebruikersAccountId == id);

        if (klant == null || klant.NatuurlijkePersoon == null || klant.NatuurlijkePersoon.GebruikersAccount == null)
        {
            return null;
        }

        klant.NatuurlijkePersoon.GebruikersAccount.Disabled = !klant.NatuurlijkePersoon.GebruikersAccount.Disabled;

        await _dbcontext.SaveChangesAsync();

        return klant;
    }
    }





