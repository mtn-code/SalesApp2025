using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Verkoop2025Data.Models;
using Verkoop2025Data.Repositories;

namespace Verkoop2025Services;
public class OrderService
{
	private readonly IOrderRepository _orderRepository;
	public OrderService(IOrderRepository orderRepository)
	{
		_orderRepository = orderRepository;
	}
	public async Task<IEnumerable<Bestelling>> GetAllOrders()
	{
		return await _orderRepository.GetAllOrdersAsync();
	}
	public async Task<Bestelling?> GetOrderDetails(int orderId)
	{
		return await _orderRepository.GetOrderByIdAsync(orderId);
	}
	public async Task<IEnumerable<Bestelling>> GetOrdersByNameAsync(string searchterm)
	{
		return await _orderRepository.GetOrdersByNameAsync(searchterm);
	}
	public async Task<IEnumerable<Bestelling>> SearchOrdersByName(string searchterm)
	{
		return await _orderRepository.SearchOrdersByNameAsync(searchterm);
	}
	public decimal TotalOrder(Bestelling order)
	{
		decimal total = 0;
		decimal discount = 1;
		if (order.ActiecodeGebruikt)
			discount = 0.9m;
		foreach (var bestellijn in order.Bestellijnen)
		{
			total += bestellijn.Artikel.Prijs * bestellijn.AantalBesteld;
		}
		return total *= discount;
	}

    public async Task UpdateOrderLineQuantity(int orderLineId, int newQuantity)
    {
        if (IsValidQuantity(newQuantity))
        await _orderRepository.UpdateOrderLineQuantityAsync(orderLineId, newQuantity);
	}

    public bool IsValidQuantity(int newQuantity)
    {
        return newQuantity >= 0;
    }
	public async Task ChangeOrderStatus(int orderId)
	{
		await _orderRepository.ChangeOrderStatusAsync(orderId);
	}
}


