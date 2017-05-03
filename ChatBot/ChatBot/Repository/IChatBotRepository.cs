using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBot.Repository
{
	public interface IChatBotRepository<T> where T : class
	{
		IEnumerable<T> GetCollection { get; }
		T FindById(string id);
		void Create(T item);
		void Update(T item);
		void Remove(T item);
		void Save();
	}
}