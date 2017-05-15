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
		public string ClientId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Notification> Notifications { get; set; }
		public virtual ICollection<Message> Messages { get; set; }
		public virtual ICollection<Attachment> Attachments { get; set; }
	}
}
