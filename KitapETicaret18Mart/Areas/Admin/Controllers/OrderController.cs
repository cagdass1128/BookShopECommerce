﻿using KitapETicaret18Mart.DataAccess.Repository;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using KitapETicaret18Mart.Models.ViewModels;
using KitapETicaret18Mart.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace KitapETicaret18Mart.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public OrderVM OrderVM { get; set; }
		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Details(int orderId)
		{
			OrderVM = new()
			{
				OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
				OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
			};

			return View(OrderVM);
		}


		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

		public IActionResult UpdateOrderDetail()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
			orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
			orderHeaderFromDb.City = OrderVM.OrderHeader.City;
			orderHeaderFromDb.State = OrderVM.OrderHeader.State;
			orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

			if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
			{
				orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			}
			if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
			{
				orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
			}

			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();

			TempData["Success"] = "Sipariş Detayları Başarıyla Güncellendi.";

			return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });

		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult StartProcessing()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
			_unitOfWork.Save();

			TempData["Success"] = "Sipariş Detayları Başarıyla Güncellendi.";

			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult ShipOrder()
		{
			var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
			orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
			orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
			orderHeader.OrderStatus = SD.StatusShipped;
			orderHeader.ShippingDate = DateTime.Now;
			if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
			{
				orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
			}

			_unitOfWork.OrderHeader.Update(orderHeader);
			_unitOfWork.Save();

			TempData["Success"] = "Sipariş Kargolandı.";

			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult CancelOrder()
		{
			//STRİPE ödeme sistemine göre entegre edilmiş.**** **** Bu servis TR'de kullanılmıyor.  **** **** 


			//var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

			//if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
			//{
			//	var options = new RefundCreateOptions
			//	{
			//		Reason = RefundReasons.RequestedByCustomer,
			//		PaymentIntent = orderHeader.PaymentIntentId
			//	};

			//	var service = new RefundService();
			//	Refund refund = service.Create(options);

			//	_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
			//}
			//else
			//{
			//	_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
			//}
			//_unitOfWork.Save();
			//TempData["Success"] = "Order Cancelled Successfully.";
			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

		}


		//[ActionName("Details")]
		//[HttpPost]
		//public IActionResult Details_PAY_NOW()
		//{ 
		//	OrderVM.OrderHeader = _unitOfWork.OrderHeader
		//		.Get(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
		//	OrderVM.OrderDetail = _unitOfWork.OrderDetail
		//		.GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProperties: "Product");

		//	//stripe logic
		//	var domain = Request.Scheme + "://" + Request.Host.Value + "/";
		//	var options = new SessionCreateOptions
		//	{
		//		SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
		//		CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
		//		LineItems = new List<SessionLineItemOptions>(),
		//		Mode = "payment",
		//	};

		//	foreach (var item in OrderVM.OrderDetail)
		//	{
		//		var sessionLineItem = new SessionLineItemOptions
		//		{
		//			PriceData = new SessionLineItemPriceDataOptions
		//			{
		//				UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
		//				Currency = "usd",
		//				ProductData = new SessionLineItemPriceDataProductDataOptions
		//				{
		//					Name = item.Product.Title
		//				}
		//			},
		//			Quantity = item.Count
		//		};
		//		options.LineItems.Add(sessionLineItem);
		//	}


		//	var service = new SessionService();
		//	Session session = service.Create(options);
		//	_unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
		//	_unitOfWork.Save();
		//	Response.Headers.Add("Location", session.Url);
		//	return new StatusCodeResult(303);
		//}

		//public IActionResult PaymentConfirmation(int orderHeaderId)
		//{

		//	OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId);
		//	if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
		//	{
		//		//this is an order by company

		//		var service = new SessionService();
		//		Session session = service.Get(orderHeader.SessionId);

		//		if (session.PaymentStatus.ToLower() == "paid")
		//		{
		//			_unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
		//			_unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
		//			_unitOfWork.Save();
		//		}
		//	}
		//	return View(orderHeaderId);
		//}



		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> objOrderHeaders;

			if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
			{
				objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
			}
			else
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

				objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
			}

			switch (status)
			{
				case "pending":
					objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
					break;
				case "inprocess":
					objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
					break;
				case "completed":
					objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
					break;
				case "approved":
					objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
					break;
				default:
					break;
			}
			return Json(new { data = objOrderHeaders });
		}







	}
}
