using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkoop2025Data.Models;

namespace Verkoop2025Data.Repositories;
public class SQLOrderRepository : IOrderRepository
{
	private readonly PrulariaComContext _dbcontext;
	public SQLOrderRepository(PrulariaComContext dbcontext)
	{
		_dbcontext = dbcontext;
	}

	public async Task<IEnumerable<Bestelling>> GetAllOrdersAsync()
	{
		return await _dbcontext.Bestellingen
			.Include(b => b.BestellingsStatus)
			.Include(b => b.Bestellijnen)
				.ThenInclude(a => a.Artikel)
            .OrderByDescending(d => d.Besteldatum)
            .ToListAsync();

	}

	public async Task<IEnumerable<Bestelling>> SearchOrdersByNameAsync(string searchterm)
	{
		return await _dbcontext.Bestellingen
								.Include(b => b.BestellingsStatus)
								.Include(b => b.Bestellijnen)
								.ThenInclude(bl => bl.Artikel)
								.AsNoTracking()
								.Where(b => b.Voornaam.Contains(searchterm) || b.Familienaam.Contains(searchterm))
								.ToListAsync();
	}

	public async Task<Bestelling?> GetOrderByIdAsync(int id)
	{
		return await _dbcontext.Bestellingen
			.Include(b => b.BestellingsStatus)
			.Include(b => b.Klant)
			   .ThenInclude(k => k.NatuurlijkePersoon)
			   .ThenInclude(np => np!.GebruikersAccount)
			.Include(b => b.Klant)
			   .ThenInclude(k => k.Rechtspersoon)
			   .ThenInclude(cp => cp!.Contactpersonen)
			   .ThenInclude(ga => ga!.GebruikersAccount)
			.Include(b => b.FacturatieAdres)
				.ThenInclude(p => p.Plaats)
			.Include(b => b.LeveringsAdres)
				.ThenInclude(p => p.Plaats)
			.Include(b => b.Bestellijnen)
			   .ThenInclude(bl => bl.Artikel)
			.Include(bw => bw.Betaalwijze)
			.FirstOrDefaultAsync(b => b.BestelId == id);
	}


	public async Task<IEnumerable<Bestelling>> GetOrdersByNameAsync(string searchterm)
	{
		return await _dbcontext.Bestellingen
			.Include(b => b.BestellingsStatus)
			.Include(b => b.Bestellijnen)
			.Where(b => b.Voornaam.Contains(searchterm) || b.Familienaam.Contains(searchterm))
			.OrderByDescending(d => d.Besteldatum)
			.ToListAsync();
	}

	public async Task<Bestelling?> ChangeOrderStatusAsync(int orderId)
	{
		var order = await _dbcontext.Bestellingen.FindAsync(orderId);
		if (order == null)
		{
			return null;
		}
		if (order.BestellingsStatusId < 3)
		{
			order.BestellingsStatusId = 3;
			order.Annulatie = true;
			order.Annulatiedatum = DateTime.Now;
			await _dbcontext.SaveChangesAsync();
		}
		else
		{
			if (order.Betaald == true)
				order.BestellingsStatusId = 2;
			else
				order.BestellingsStatusId = 1;
			order.Annulatie = false;
			order.Annulatiedatum = null;
			await _dbcontext.SaveChangesAsync();
		}
		return order;
	}


	public async Task UpdateOrderLineQuantityAsync(int orderLineId, int newQuantity)
	{
		var orderLine = await _dbcontext.Bestellijnen.FindAsync(orderLineId);
		orderLine.AantalBesteld = newQuantity;
		await _dbcontext.SaveChangesAsync();
	}


}

