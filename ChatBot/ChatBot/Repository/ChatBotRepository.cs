using ChatBot.Infrastructure;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace ChatBot.Repository
{
	public class ChatBotRepository<T> : IDisposable, IChatBotRepository<T>
		where T : class
	{
		private ChatBotContext _chatBotContext;

		private DbSet<T> _dbSet;

		private Type classType;
		private bool disposed = false;

		public IEnumerable<T> GetCollection
		{
			get
			{
				return _dbSet.AsNoTracking().ToList();
			}
		}

		public ChatBotRepository(ChatBotContext chatBotContext)
		{
			_chatBotContext = chatBotContext;
			_dbSet = _chatBotContext.Set<T>();
		}

		public T FindById(string id)
		{
			return _dbSet.Find(id);
		}

		public void Create(T item)
		{
			_dbSet.Add(item);
			_chatBotContext.SaveChanges();
		}
		public void Update(T item)
		{
			_chatBotContext.Entry(item).State = EntityState.Modified;
			_chatBotContext.SaveChanges();
		}
		public void Remove(T item)
		{
			_dbSet.Remove(item);
			_chatBotContext.SaveChanges();
		}

		public void Save()
		{
			_chatBotContext.SaveChanges();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_chatBotContext.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}