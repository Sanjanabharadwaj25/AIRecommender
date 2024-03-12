using AIRec.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRec.Cache
{
    public class BooksDataService
    {
        private readonly IDataLoader _dataLoader;
        private readonly IDataCacher _dataCacher;

        public BooksDataService(IDataLoader dataLoader, IDataCacher dataCacher)
        {
            _dataLoader = dataLoader;
            _dataCacher = dataCacher;
        }

        public BookDetails GetBookDetails()
        {
            var cacheKey = "bookDetails";
            var bookDetails = _dataCacher.GetData(cacheKey);
            if (bookDetails == null)
            {
                // Data not in cache, load from data source
                Console.WriteLine("***Data not found in cache. Loading from the data source.***");
                bookDetails = _dataLoader.Load("Books.csv", "Users.csv", "Ratings.csv");
                _dataCacher.SetData(cacheKey, bookDetails);
            }
            else
            {
                Console.WriteLine("***Data retrieved from cache.***");
            }
            return bookDetails;
        }
    }

    //public class BooksDataService
    //{
    //    private readonly IDataLoader _dataLoader;
    //    private readonly IDataCacher _dataCacher;

    //    public BooksDataService(IDataLoader dataLoader, IDataCacher dataCacher)
    //    {
    //        _dataLoader = dataLoader;
    //        _dataCacher = dataCacher;
    //    }

    //    public async Task<BookDetails> GetBookDetailsAsync()
    //    {
    //        var cacheKey = "bookDetails";
    //        var bookDetails = await _dataCacher.GetDataAsync(cacheKey);

    //        if (bookDetails == null)
    //        {
    //            Console.WriteLine("***Data not found in cache. Loading from the data source.***");
    //            bookDetails = _dataLoader.Load("Books.csv", "Users.csv", "Ratings.csv");
    //            await _dataCacher.SetDataAsync(cacheKey, bookDetails);
    //        }
    //        else
    //        {
    //            Console.WriteLine("***Data retrieved from cache.***");
    //        }

    //        return bookDetails;
    //    }
    //}

}
