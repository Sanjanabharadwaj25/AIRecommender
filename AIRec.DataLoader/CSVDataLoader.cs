using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using AIRec.Common;


namespace AIRec.DataLoader
{
    public class BookDetails
    {
        public Dictionary<string, Book> BooksDict { get; set; } = new Dictionary<string, Book>();
        public List<BookUserRating> UserRatings { get; set; } = new List<BookUserRating>();
        public Dictionary<string, User> UsersDict { get; set; } = new Dictionary<string, User>();
    }

    public interface IDataLoader
    {
        BookDetails Load(string booksFilePath, string usersFilePath, string ratingsFilePath);
    }

    public class CSVDataLoader : IDataLoader
    {
        public BookDetails Load(string booksFilePath, string usersFilePath, string ratingsFilePath)
        {
            BookDetails bookDetails = new BookDetails();
            Task.WaitAll(
                Task.Run(() => LoadBooks(booksFilePath, bookDetails)),
                Task.Run(() => LoadUsers(usersFilePath, bookDetails)),
                Task.Run(() => LoadRatings(ratingsFilePath, bookDetails))
            );
            return bookDetails;
        }

        private void LoadBooks(string booksFilePath, BookDetails bookDetails)
        {
            var line = "";

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(booksFilePath);

                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(';');
                    values[0] = values[0].Trim('"');
                    Book book = new Book(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
                    bookDetails.BooksDict.Add(values[0], book);
                }
            }
            catch (NullReferenceException e)
            {

            }
            finally
            {
                reader.Close();
            }
        }
        private void LoadUsers(string usersFilePath, BookDetails bookDetails)
        {
            var line = "";

            StreamReader reader = null;
            reader = new StreamReader(usersFilePath);
            reader.ReadLine();
            try
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(';');
                    if (values.Length != 3) continue;

                    values[2] = (values[2] == "NULL") ? "0" : values[2].Trim('"');


                    var location = values[1].Split(',');
                    if (location.Length != 3) continue;
                    User user = new User(values[0], location[0], location[1], location[2], values[2]);

                    bookDetails.UsersDict.Add(values[0], user);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        private void LoadRatings(string ratingsFilePath, BookDetails bookDetails)
        {
            var line = "";

            StreamReader reader = null;
            reader = new StreamReader(ratingsFilePath);
            reader.ReadLine();
            try
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(';');
                    if (values.Length != 3) continue;
                    values[1] = values[1].Trim('"');
                    values[2] = values[2].Trim('"');

                    BookUserRating rating = new BookUserRating(values[0], values[1], values[2]);
                    bookDetails.UserRatings.Add(rating);

                    if (bookDetails.BooksDict.ContainsKey(values[1]))
                        bookDetails.BooksDict[values[1]].bookUserRatings.Add(rating);

                    if (bookDetails.UsersDict.ContainsKey(values[0]))
                        bookDetails.UsersDict[values[0]].bookUserRatings.Add(rating);
                }
            }
            finally
            {
                reader.Close();
            }

        }
    }
 
}

