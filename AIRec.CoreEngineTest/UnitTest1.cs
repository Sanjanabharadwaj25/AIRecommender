using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AIRec.DataLoader;
using AIRec.Common;
using AIRec.CoreEngine;
namespace AIRec.CoreEngineTest
{
    [TestClass]
    public class DataLoaderUnitTests
    {

        private readonly string testBooksFilePath = "TestBooks.csv";
        private readonly string testUsersFilePath = "TestUsers.csv";
        private readonly string testRatingsFilePath = "TestRatings.csv";

        [TestMethod]
        public void LoadValidData_ReturnsBookDetails()
        {

            CSVDataLoader dataLoader = new CSVDataLoader();


            BookDetails bookDetails = dataLoader.Load(testBooksFilePath, testUsersFilePath, testRatingsFilePath);


            Assert.IsNotNull(bookDetails);
            Assert.IsNotNull(bookDetails.BooksDict);
            Assert.IsNotNull(bookDetails.UserRatings);
            Assert.IsNotNull(bookDetails.UsersDict);
        }

        
    }

    [TestClass]
    public class PearsonRecommenderTests
    {
        [TestMethod]
        public void GetCorrelation_ShouldReturnCorrectly()
        {
            
            IDataPreprocessor preprocessor = new DataPreprocessor();
            IRecommender recommender = PearsonRecommender.Create(preprocessor);

            
            int[] baseData = { 1, 2, 3, 4, 5 };
            int[] otherData = { 5, 4, 3, 2, 1 };

           
            double correlation = recommender.GetCorrelation(baseData, otherData);

           
            Assert.AreEqual(-1.0, correlation, 0.001, "Unexpected Pearson correlation coefficient.");
        }



        [TestMethod]
        public void GetCorrelation_PositiveCorrelation_ShouldReturnCofficientValue()
        {
          
            IDataPreprocessor preprocessor = new DataPreprocessor();
            IRecommender recommender = PearsonRecommender.Create(preprocessor);

            int[] baseData = { 1, 2, 3, 4, 5 };
            int[] otherData = { 2, 4, 6, 8, 10 };

            double correlation = recommender.GetCorrelation(baseData, otherData);
            Assert.AreEqual(1.0, correlation, 0.001, "Unexpected Pearson correlation coefficient.");
        }

       
    }

}
