using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRec.Common
{
	public class BookUserRating
	{
		public string Rating { get; set; }
		public string ISBN { get; set; }
		public string UserID { get; set; }

		public BookUserRating(string userID, string iSBN, string rating)
		{
			Rating = rating;
			ISBN = iSBN;
			UserID = userID;
		}
	}
}
