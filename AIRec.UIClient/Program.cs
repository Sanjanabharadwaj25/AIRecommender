using AIRec.Common;
using AIRec.CoreEngine;
using AIRec.DaaAggregator;
using AIRec.DataLoader;
using AIRec.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRec.UIClient
{
    public class AIRecommendationEngine
    {
        private readonly IDataLoader _dataLoader;
        private readonly IRatingsAggregator _ratingsAggregator;
        private readonly IRecommender _recommender;

        public AIRecommendationEngine(IDataLoader dataLoader, IRatingsAggregator ratingsAggregator, IRecommender recommender)
        {
            _dataLoader = dataLoader;
            _ratingsAggregator = ratingsAggregator;
            _recommender = recommender;
        }

        public List<Book> Recommend(Preference preference, int limit)
        {
            // Console.WriteLine("reading");
            var bookDetailsList = _dataLoader.Load("Books.csv", "Users.csv", "Ratings.csv");
            //Console.WriteLine("read the data");
            var aggregatedRatings = _ratingsAggregator.Aggregate(bookDetailsList, preference);
            // Console.WriteLine(aggregatedRatings.Count);

            List<int> preferencedRatings = new List<int>();
            if (bookDetailsList.BooksDict.ContainsKey(preference.ISBN))
            {
                preferencedRatings = bookDetailsList.BooksDict[preference.ISBN].bookUserRatings
                                    .Select(rating => int.Parse(rating.Rating))
                                    .ToList();
            }
            //Console.WriteLine(aggregatedRatings);
            var scores = new Dictionary<string, double>();
            foreach (var entry in aggregatedRatings)
            {
                if (entry.Key != preference.ISBN)
                {
                    var otherRatings = entry.Value.Select(rating => {
                        if (rating is int intValue)
                            return intValue;
                        return 0;
                    }).ToArray();
                    double score = _recommender.GetCorrelation(preferencedRatings.ToArray(), otherRatings);
                    scores[entry.Key] = score;
                }
            }

            var recommendedISBNs = scores.OrderByDescending(pair => pair.Value).Take(limit).Select(pair => pair.Key);

            List<Book> recommendedBooks = recommendedISBNs
                .Where(isbn => bookDetailsList.BooksDict.ContainsKey(isbn))
                .Select(isbn => bookDetailsList.BooksDict[isbn])
                .ToList();

            // Console.WriteLine(recommendedBooks.Count);
            return recommendedBooks;
        }
    }


    public class Client
    {
        public Preference UserPreference { get; set; }
    }

    class Program
    {
        static void Main()
        {
            string connectionString = "localhost: 6379,password = mypassword,abortConnect = false,defaultDatabase = 0";
                Console.WriteLine("ISBN of the book to order: ");
                string isbn = Console.ReadLine();
                Console.WriteLine("State of the user: ");
                string state = Console.ReadLine();
                Console.WriteLine("Age of the user: ");
                int age = int.Parse(Console.ReadLine());
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                IDataLoader dataLoader = new CSVDataLoader();
                IDataCacher datacacher = new MemDataCacher(connectionString);
                IRatingsAggregator ratingsAggregator = new RatingsAggregator();
                IRecommender recommender = RecommenderFactory.CreateRecommender();
                AIRecommendationEngine recommendationEngine = new AIRecommendationEngine(dataLoader, ratingsAggregator, recommender);
               BooksDataService booksDataService = new BooksDataService(dataLoader, datacacher);
               var bookDetails = booksDataService.GetBookDetails();
                Client client = new Client
                {
                    UserPreference = new Preference
                    {
                        ISBN = isbn,
                        State = state,
                        Age = age
                    }
                };
                //Console.WriteLine("Got the preference");
                int numberOfRecommendations = 10;
                List<Book> recommendations = recommendationEngine.Recommend(client.UserPreference, numberOfRecommendations);
                //Console.WriteLine("sent the preference");
                stopwatch.Stop();
                foreach (var book in recommendations)
                {

                    Console.WriteLine($"Title: {book.BookTitle}");
                    Console.WriteLine($"Author: {book.BookAuthor}");
                    Console.WriteLine();

                }

                Console.WriteLine("The time taken is " + stopwatch.ElapsedMilliseconds + "ms");
               
        }
    }
}
