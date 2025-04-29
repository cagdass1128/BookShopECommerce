using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapETicaret18Mart.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext applicationDbContext;
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }

		public UnitOfWork(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
			Category = new CategoryRepository(applicationDbContext);
			Product = new ProductRepository(applicationDbContext);
			Company = new CompanyRepository(applicationDbContext);
			ShoppingCart = new ShoppingCartRepository(applicationDbContext);
			ApplicationUser = new ApplicationUserRepository(applicationDbContext);
			OrderDetail = new OrderDetailRepository(applicationDbContext);
			OrderHeader = new OrderHeaderRepository(applicationDbContext);
		}


		public void Save()
		{
			applicationDbContext.SaveChanges();
		}


	}
}
