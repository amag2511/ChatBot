using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	[Serializable]
	public class MediaElement
	{
		public int Id { get; set; }
		public string Tag { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public byte[] ContentData { get; set; }
		public string ContentUrl => $"data:{ContentType};base64,{ContentData}";

		public int? UserId { get; set; }
		public virtual User User { get; set; }
	}
}
