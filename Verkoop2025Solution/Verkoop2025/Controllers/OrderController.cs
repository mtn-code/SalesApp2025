using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Verkoop2025Services;
using Verkoop2025Web.Models;
using X.PagedList.Extensions;


namespace Verkoop2025Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly OrderService _orderService;
		private const int PageSize = 5;
		public OrderController(OrderService orderService)
		{
			_orderService = orderService;
		}


		[HttpGet]
		public async Task<IActionResult> Index(int page = 1)
		{
			try
			{
				var orders = await _orderService.GetAllOrders();
				var pagedOrders = orders.ToPagedList(page, PageSize);

				var viewModel = new OrderViewModel
				{
					Bestellingen = pagedOrders
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching alle orders: {ex.Message}");
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> SearchOrder(string searchterm, int page = 1)
		{
			try
			{
				var orders = await _orderService.SearchOrdersByName(searchterm);
				var pagedOrders = orders.ToPagedList(page, PageSize);

				var viewModel = new OrderViewModel
				{
					Bestellingen = pagedOrders
				};

				return View("Index", viewModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching order naam: {ex.Message}");
				return View();
			}
		}


		[HttpGet]
		public async Task<IActionResult> Details(int id, int page = 1)
		{
			try
			{
				var orderDetail = await _orderService.GetOrderDetails(id);
				if (orderDetail == null)
				{
					return NotFound();
				}

				var pagedBestellijnen = orderDetail.Bestellijnen.ToPagedList(page, PageSize);

				var orderDetailViewModel = new OrderDetailViewModel()
				{
					BestelId = orderDetail.BestelId,
					BestelDatum = orderDetail.Besteldatum,
					BestellingStatus = orderDetail.BestellingsStatus.Naam,
					KlantVoornaam = orderDetail.Klant?.NatuurlijkePersoon?.Voornaam,
					KlantFamilienaam = orderDetail.Klant?.NatuurlijkePersoon?.Familienaam,
					KlantEmail = orderDetail.Klant?.NatuurlijkePersoon?.GebruikersAccount.Emailadres,
					Facturatieadres = orderDetail.FacturatieAdres,
					Leveradres = orderDetail.LeveringsAdres,
					Bestellijnen = pagedBestellijnen!,
					Contactpersonen = orderDetail.Klant?.Rechtspersoon?.Contactpersonen,
					RechtspersoonBTWNr = orderDetail.BtwNummer,
					RechtspersoonNaam = orderDetail.Klant?.Rechtspersoon?.Naam,
					Betaalwijze = orderDetail.Betaalwijze,
					Betalingscode = orderDetail.Betalingscode,
					ActiecodeGebruikt = orderDetail.ActiecodeGebruikt
				};

                return View(orderDetailViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching orders: {ex.Message}");
                return View();
            }
        }
        [HttpPost]
		public async Task<IActionResult> UpdateOrderLineQuantity(int orderId, int orderLineId, int newQuantity)
		{
			try
			{
				await _orderService.UpdateOrderLineQuantity(orderLineId, newQuantity);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error updating quatity: {ex.Message}");
			}
			return RedirectToAction("Details", new { id = orderId });
		}

	[HttpGet]
		public async Task<IActionResult> ChangeOrderStatus(int orderId)
		{
			try
			{

				var order = await _orderService.GetOrderDetails(orderId);
				if (order == null)
				{
					return NotFound($"Order with ID {orderId} not found.");
				}
				if (order.BestellingsStatusId < 3)
					await _orderService.ChangeOrderStatus(orderId);
				else
					return BadRequest("Order status cannot be changed.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error changing orderstatus: {ex.Message}");
				return StatusCode(500, "Internal server error.");
			}
			return RedirectToAction("Details", new { id = orderId });
		}
	}
}

