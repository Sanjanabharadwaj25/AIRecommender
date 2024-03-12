using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRec.DataLoader;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace AIRec.Cache
{
    public interface IDataCacher
    {
        BookDetails GetData(string key);
        void SetData(string key, BookDetails data);
    }
    public class MemDataCacher : IDataCacher
    {
        private readonly IDatabase _database;

        public MemDataCacher(string connectionString)
        {
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _database = connection.GetDatabase();
        }

        public BookDetails GetData(string key)
        {
            var cachedData = _database.StringGet(key);
            if (!cachedData.IsNull)
            {
                //Console.WriteLine($"Data retrieved from Redis cache for key: {key}");
                return JsonConvert.DeserializeObject<BookDetails>(cachedData);
            }

            //Console.WriteLine($"No data found in Redis cache for key: {key}");
            return null;
        }

        public void SetData(string key, BookDetails data)
        {
            Console.WriteLine($"***Caching Data to Redis for key: {key}***");
            var jsonData = JsonConvert.SerializeObject(data);
            _database.StringSet(key, jsonData, TimeSpan.FromMilliseconds(10000));
        }
    }

    //public interface IDataCacher
    //{
    //    Task<BookDetails> GetDataAsync(string key);
    //    Task SetDataAsync(string key, BookDetails data);
    //}

    //public class MemDataCacher : IDataCacher
    //{
    //    private readonly IDatabase _database;

    //    public MemDataCacher(string connectionString)
    //    {
    //        var connection = ConnectionMultiplexer.Connect(connectionString);
    //        _database = connection.GetDatabase();
    //    }

    //    public async Task<BookDetails> GetDataAsync(string key)
    //    {
    //        var cachedData = await _database.StringGetAsync(key);
    //        return !cachedData.IsNull ? JsonConvert.DeserializeObject<BookDetails>(cachedData) : null;
    //    }

    //    public async Task SetDataAsync(string key, BookDetails data)
    //    {
    //        var jsonData = JsonConvert.SerializeObject(data);
    //        await _database.StringSetAsync(key, jsonData, TimeSpan.FromMinutes(10));
    //    }
    //}



}
