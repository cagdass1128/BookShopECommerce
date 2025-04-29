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
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext applicationDbContext;
		public OrderHeaderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) 
		{        
			this.applicationDbContext = applicationDbContext;
		}
		     

		public void Update(OrderHeader obj)
		{
			applicationDbContext.OrderHeaders.Update(obj);
		}
	}
}
