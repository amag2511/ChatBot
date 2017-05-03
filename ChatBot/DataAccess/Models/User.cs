using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public virtual List<Notification> Notifications { get; set; }
		public virtual List<Message> Messages { get; set; }
		public virtual List<Attachment> Attachments { get; set; }
	}
}
