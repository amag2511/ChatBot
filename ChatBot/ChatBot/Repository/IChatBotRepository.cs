﻿using DataAccess.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ChatBot.Repository
{
	public interface IChatBotRepository<T> where T : class
	{
		IEnumerable<T> GetCollection { get; }
		T FindById(int id);
		void Create(T item);
		void Update(T item);
		void Remove(T item);
		IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
		IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
			params Expression<Func<T, object>>[] includeProperties);

		User GetSender(string conversationId, string userName);
		void Save();
	}
}