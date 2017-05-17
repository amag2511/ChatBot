using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	[Serializable]
	public class User
	{
		public int Id { get; set; }
		public string ConversationId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Notification> Notifications { get; set; }
		public virtual ICollection<MediaElement> MediaElements { get; set; }
	}
}
