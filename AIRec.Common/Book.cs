using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRec.Common
{
    public class Book
    {
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string YearOfPublication { get; set; }
        public string ImageUrlSmall { get; set; }
        public string ImageUrlMedium { get; set; }
        public string ImageUrlLarge { get; set; }
        public List<BookUserRating> bookUserRatings { get; set; } = new List<BookUserRating>();

        public Book(string iSBN, string bookTitle, string bookAuthor, string yearOfPublication, string publisher, string imageUrlSmall, string imageUrlMedium, string imageUrlLarge)
        {
            BookTitle = bookTitle;
            BookAuthor = bookAuthor;
            ISBN = iSBN;
            Publisher = publisher;
            YearOfPublication = yearOfPublication;
            ImageUrlSmall = imageUrlSmall;
            ImageUrlMedium = imageUrlMedium;
            ImageUrlLarge = imageUrlLarge;
        }
    }
}
