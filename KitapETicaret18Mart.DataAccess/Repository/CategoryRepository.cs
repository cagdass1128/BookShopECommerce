using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using KitapETicaret18Mart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitapETicaret18Mart.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext applicationDbContext;
		public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) 
		{        
			this.applicationDbContext = applicationDbContext;
		}
		     

		public void Update(Category obj)
		{
			applicationDbContext.Categories.Update(obj);
		}
	}
}
