using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	[Serializable]
	public class Notification
	{
		public int Id { get; set; }
		public TimeSpan Time { get; set; }
		public string Description { get; set; }

		public int? UserId { get; set; }
		public virtual User User { get; set; }
	}
}
