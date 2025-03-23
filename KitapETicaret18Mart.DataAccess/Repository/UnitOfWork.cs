﻿using KitapETicaret18Mart.DataAccess.Data;
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
		public UnitOfWork(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
			Category = new CategoryRepository(applicationDbContext);
		}
		

		public void Save()
		{
			applicationDbContext.SaveChanges();
		}
	}
}
