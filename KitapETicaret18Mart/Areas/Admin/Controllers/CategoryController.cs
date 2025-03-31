using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using Microsoft.AspNetCore.Mvc;

namespace KitapETicaret18Mart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Category> categoryList = unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Kategori Adı ve Numara Aynı Olamaz!");
            }

            if (category.Name != null && category.Name.ToLower() == "")
            {
                ModelState.AddModelError("", "Geçerli Bir Kategori Adı Giriniz");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Category.Add(category);
                unitOfWork.Save();
                TempData["success"] = "Kategori Başarıyla Eklendi";
                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = unitOfWork.Category.Get(u => u.Id == categoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category editCategory)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Category.Update(editCategory);
                unitOfWork.Save();
                TempData["success"] = "Kategori Başarıyla Güncellendi";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = unitOfWork.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = unitOfWork.Category.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            unitOfWork.Category.Remove(obj);
            unitOfWork.Save();
            TempData["success"] = "Kategori Başarıyla Silindi";
            return RedirectToAction("Index");

        }









    }
}
