using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRec.Common;
using AIRec.DataLoader;

namespace AIRec.DaaAggregator
{
    public class Preference
    {
        public string ISBN { get; set; }
        public string State { get; set; }
        public int Age { get; set; }
    }

    public interface IRatingsAggregator
    {
        Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preference preference);
    }

    public class RatingsAggregator : IRatingsAggregator
    {
        public Dictionary<string, List<int>> Aggregate(BookDetails bookDetails, Preference preference)
        {

            Dictionary<string, List<int>> ratingsForABook = new Dictionary<string, List<int>>();
            var users = bookDetails.UsersDict.Values.ToList();
            string expectedState = preference.State.ToLower().Trim();
            int expectedAge = preference.Age;
            int userAge, userBookRating;
            string userState;

            foreach (User user in users)
            {
                userState = user.State.ToLower().Trim();
                if (int.TryParse(user.Age, out userAge) && userState.Contains(expectedState) && IsWithinAgeGroup(userAge, expectedAge))
                {
                    foreach (BookUserRating bookRating in user.bookUserRatings)
                    {
                        if (!ratingsForABook.ContainsKey(bookRating.ISBN))
                            ratingsForABook[bookRating.ISBN] = new List<int>();

                        if (int.TryParse(bookRating.Rating, out userBookRating))
                            ratingsForABook[bookRating.ISBN].Add(userBookRating);
                    }
                }

            }

            return ratingsForABook;
        }

        private bool IsWithinAgeGroup(int userAge, int targetAge)
        {
            // int userAge = int.Parse(SuserAge);
            return CheckAgeGroup(userAge) == CheckAgeGroup(targetAge);
        }

        private string CheckAgeGroup(int age)
        {
            if (age <= 16)
                return "Teen Age";
            else if (age <= 30)
                return "Young Age";
            else if (age <= 50)
                return "Mid Age";
            else if (age <= 60)
                return "Old Age";
            else
                return "Senior Citizens";
        }
    }
}

