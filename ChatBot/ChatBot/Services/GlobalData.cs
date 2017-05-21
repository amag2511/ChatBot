using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBot.Services
{
	[Serializable]
	public class GlobalData
	{
		private static readonly Lazy<GlobalData> lazy =
				new Lazy<GlobalData>(() => new GlobalData());

		public NotifyService NotifyService { get; set; }
		public string Name { get; private set; }

		private GlobalData()
		{
			Name = System.Guid.NewGuid().ToString();
			NotifyService = new NotifyService();
		}

		public static GlobalData GetInstance()
		{
			return lazy.Value;
		}
	}
}