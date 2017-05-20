using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	[Serializable]
	public class MediaElement
	{
		private string ContentUrl => $"data:{ContentType};base64,";

		public int Id { get; set; }
		public string Tag { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public byte[] ContentData { get; set; }

		public int? UserId { get; set; }
		public virtual User User { get; set; }

		public string GetContentUrl()
		{
			return ContentUrl + Convert.ToBase64String(ContentData);
		}
	}
}
