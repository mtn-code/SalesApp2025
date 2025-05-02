using Verkoop2025Data.Models;

namespace Verkoop2025Data.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Bestelling>> GetAllOrdersAsync();
        Task<Bestelling?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Bestelling>> GetOrdersByNameAsync(string searchterm);
        Task<IEnumerable<Bestelling>> SearchOrdersByNameAsync(string searchterm);
		Task<Bestelling?> ChangeOrderStatusAsync(int orderId);
        Task UpdateOrderLineQuantityAsync(int orderLineId, int newQuantity);
	}
}
