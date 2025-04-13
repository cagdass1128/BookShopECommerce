using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using KitapETicaret18Mart.Models.ViewModels;
using KitapETicaret18Mart.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KitapETicaret18Mart.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork unitOfWork;		

		public CompanyController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;			
		}


		public IActionResult Index()
		{
			List<Company> CompanyList = unitOfWork.Company.GetAll().ToList();
			return View(CompanyList);
		}


		public IActionResult Upsert(int? id)
		{
			if (id == null || id == 0)
			{
				//Create
				return View(new Company());
			}
			else
			{
				//Update
				Company companyObj = unitOfWork.Company.Get(u => u.Id == id);
				return View(companyObj);
			}
		}

		[HttpPost]
		public IActionResult UpSert(Company CompanyObj)
		{

			if (ModelState.IsValid)
			{
				if (CompanyObj.Id == 0)
				{
					unitOfWork.Company.Add(CompanyObj);
				}
				else
				{
					unitOfWork.Company.Update(CompanyObj);
				}

				unitOfWork.Save();
				TempData["success"] = "Kategori Başarıyla Eklendi";
				return RedirectToAction("Index");
			}
			else
			{
				return View(CompanyObj);
			}
		}

		#region API 
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Company> CompanyList = unitOfWork.Company.GetAll().ToList();
			return Json(new { data = CompanyList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var CompanyToBeDeleted = unitOfWork.Company.Get(x => x.Id == id);
			if (CompanyToBeDeleted == null)
			{
				return Json(new { success = false, message = "Silinme Hatası Oluştu" });
			}			
			unitOfWork.Company.Remove(CompanyToBeDeleted);
			unitOfWork.Save();


			return Json(new { success = true, message = "Başarıyla Silindi" });
		}

		#endregion







	}
}
