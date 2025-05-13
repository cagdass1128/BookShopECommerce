using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using KitapETicaret18Mart.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KitapETicaret18Mart.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;

		public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
			this.unitOfWork = unitOfWork;
		}

        public IActionResult Index()
        {
			IEnumerable<Product> productList = unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

		public IActionResult Details(int productId)
		{
            ShoppingCart cart = new()
            {
                Product = unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };	
			return View(cart);
		}


        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //ClaimsIdentity yardýmcý methodtur. Identity package'tan gelir.User ýd verir.
            var claimsIdentity = (ClaimsIdentity)User.Identity;            
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId && x.ProductId==shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;                
                unitOfWork.ShoppingCart.Update(cartFromDb);
				unitOfWork.Save();
			}
            else
            {
                //add cart record
                unitOfWork.ShoppingCart.Add(shoppingCart);
				unitOfWork.Save();
				HttpContext.Session.SetInt32(SD.SessionCart, unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Sepet Baþarýyla Güncellendi";
            

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
