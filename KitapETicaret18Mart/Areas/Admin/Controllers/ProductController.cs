using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using KitapETicaret18Mart.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KitapETicaret18Mart.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			this.unitOfWork = unitOfWork;
			this.webHostEnvironment = webHostEnvironment;
		}


		public IActionResult Index()
		{
			List<Product> ProductList = unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
			return View(ProductList);
		}


		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				Product = new Product()
			};

			if (id == null || id == 0)
			{
				//Create
				return View(productVM);
			}
			else
			{
				//Update
				productVM.Product = unitOfWork.Product.Get(u => u.Id == id);
				return View(productVM);
			}


		}

		[HttpPost]
		public IActionResult UpSert(ProductVM productVM, IFormFile file)
		{

			if (ModelState.IsValid)
			{
				string wwwRootPath = webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\product");

					if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
					{
						//Delete the Old image
						var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}


					using ( var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					productVM.Product.ImageUrl = @"\images\product\" + fileName;
				}

				if (productVM.Product.Id == 0)
				{
					unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					unitOfWork.Product.Update(productVM.Product);
				}				

				unitOfWork.Save();
				TempData["success"] = "Kategori Başarıyla Eklendi";
				return RedirectToAction("Index");
			}
			else
			{
				productVM.CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(productVM);
			}

		}

		#region API 
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Product> ProductList = unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = ProductList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var productToBeDeleted = unitOfWork.Product.Get(x => x.Id == id);
			if(productToBeDeleted == null)
			{
				return Json(new { success = false, message = "Silinme Hatası Oluştu" });
			}

			var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

			unitOfWork.Product.Remove(productToBeDeleted);
			unitOfWork.Save();


			return Json(new { success=true, message= "Başarıyla Silindi" });
		}

		#endregion







	}
}
