using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class Attachment
	{
		public int Id { get; set; }
		public string Tag { get; set; }
		public string UriAttachment { get; set; }
		public string Description { get; set; }
		public virtual User User { get; set; }

	}
}
