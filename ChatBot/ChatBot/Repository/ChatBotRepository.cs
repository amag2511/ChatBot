using ChatBot.Infrastructure;
using DataAccess.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ChatBot.Repository
{
	[Serializable]
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

		public ChatBotRepository()
		{
			_chatBotContext = new ChatBotContext();
			_dbSet = _chatBotContext.Set<T>();
		}

		public T FindById(int id)
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

		public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
		{
			return Include(includeProperties).ToList();
		}

		public IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
			params Expression<Func<T, object>>[] includeProperties)
		{
			var query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();
			return includeProperties
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		public User GetSender(IMessageActivity activity)
		{
			var currentObject = (this as ChatBotRepository<User>);

			if (currentObject == null)
			{
				return null;
			}

			return (this as ChatBotRepository<User>).GetWithInclude(x => x.MediaElements, x => x.Notifications)
					.FirstOrDefault(x => x.ConversationId == activity.Conversation.Id && x.ToName == activity.From.Name);

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