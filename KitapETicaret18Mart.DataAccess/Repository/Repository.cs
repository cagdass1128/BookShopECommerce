using KitapETicaret18Mart.DataAccess.Data;
using KitapETicaret18Mart.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KitapETicaret18Mart.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		internal DbSet<T> dbSet;
		private readonly ApplicationDbContext applicationDb;
		public Repository(ApplicationDbContext applicationDb){
			this.applicationDb = applicationDb;
			this.dbSet = applicationDb.Set<T>();
			//applicationDb.Categories == dbSet ||| yani --> dbSet.Add()
		}

		public void Add(T entity)
		{
			dbSet.Add(entity);		
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = dbSet;
			query = query.Where(filter);
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetAll()
		{
			IQueryable<T> query = dbSet;
			return query.ToList();
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			dbSet.RemoveRange(entity);
		}
	}
}
