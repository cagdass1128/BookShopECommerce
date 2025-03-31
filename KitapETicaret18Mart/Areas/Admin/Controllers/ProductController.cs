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
			List<Product> ProductList = unitOfWork.Product.GetAll().ToList();
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

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? ProductFromDb = unitOfWork.Product.Get(u => u.Id == id);

			if (ProductFromDb == null)
			{
				return NotFound();
			}
			return View(ProductFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = unitOfWork.Product.Get(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}
			unitOfWork.Product.Remove(obj);
			unitOfWork.Save();
			TempData["success"] = "Kategori Başarıyla Silindi";
			return RedirectToAction("Index");

		}









	}
}
