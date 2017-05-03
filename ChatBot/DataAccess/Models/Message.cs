using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class Message
	{
		public string Id { get; set; }
		public string Tag { get; set; }
		public string BotsMessage { get; set; }
		public virtual User User { get; set; }

	}
}
