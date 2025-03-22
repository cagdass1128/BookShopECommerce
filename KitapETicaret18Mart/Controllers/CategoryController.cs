using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.Models;
using Microsoft.AspNetCore.Mvc;

namespace KitapETicaret18Mart.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public IActionResult Index()
        {
            List<Category> categoryList = applicationDbContext.Categories.ToList();
            return View(categoryList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
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
                await applicationDbContext.Categories.AddAsync(category);
                await applicationDbContext.SaveChangesAsync();
                TempData["success"] = "Kategori Başarıyla Eklendi";
                return RedirectToAction("Index");
            }

            return View();
        }


        public async Task<IActionResult> Edit(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = await applicationDbContext.Categories.FindAsync(categoryId);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category editCategory)
        {
            if (ModelState.IsValid)
            {
                applicationDbContext.Categories.Update(editCategory);
                await applicationDbContext.SaveChangesAsync();
                TempData["success"] = "Kategori Başarıyla Güncellendi";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = await applicationDbContext.Categories.FindAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Category? obj = await applicationDbContext.Categories.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            applicationDbContext.Categories.Remove(obj);
            await applicationDbContext.SaveChangesAsync();
            TempData["success"] = "Kategori Başarıyla Silindi";
            return RedirectToAction("Index");

        }









    }
}
