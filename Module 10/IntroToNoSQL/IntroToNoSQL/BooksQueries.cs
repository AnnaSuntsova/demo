using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace IntroToNoSQL
{
    class BooksQueries
    {       
        public IMongoCollection<Book> Collection;

        public void SetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase("Library");
            Collection = mongoDatabase.GetCollection<Book>("Books");

        }

        public List<Book> Add (List<Book> books)
        {
            Collection.InsertMany(books);
            return books;
        }

        public IOrderedFindFluent<Book, Book> FindBooksWithCountGreater1 ()
        {
            return Collection.Find(book => book.Count > 1).SortBy(book => book.Name); 
        }

        public List <Book> Find3BooksWithCountGreater1()
        {
            var booksList = FindBooksWithCountGreater1();
            return booksList.Limit(3).ToList();            
        }

        public long FindBooksAmountWithCountGreater1()
        {
            var booksList = FindBooksWithCountGreater1();
            return booksList.CountDocuments();
        }

        public Book FindBookWithMinCount()
        {   
            var sort = Builders<Book>.Sort.Descending("Count");
            return Collection.Find(book=>true).Sort(sort).FirstOrDefault();
        }

        public Book FindBookWithMaxCount()
        {
            var sort = Builders<Book>.Sort.Ascending("Count");
            return Collection.Find(x => true).Sort(sort).FirstOrDefault();
        }

        public IEnumerable<string> GetAuthors()
        {
            var filter = Builders<Book>.Filter.Ne(x => x.Author, null);
            return Collection.Distinct(x => x.Author, filter).ToList();
        }

        public IEnumerable<Book> GetBooksWithoutAuthors()
        {
            var filter = Builders<Book>.Filter.Eq(x => x.Author, null);
            return Collection.Find(filter).ToList();
        }

        public void IncreaseCount()
        {
            var update = Builders<Book>.Update.Inc("Count", 1);
            var filter = Builders<Book>.Filter.Empty;
            Collection.UpdateMany(filter, update);
        }

        public void AddGenre ()
        {
            var filter = Builders<Book>.Filter.Where(x => x.Genre.Contains("fantasy") && !x.Genre.Contains("favority"));
            var update = Builders<Book>.Update.Set(x => x.Genre[1], "favority");
            Collection.UpdateMany(filter, update);
        }               

        public void DeleteBooksWithCountLower3()
        {
            var filter = Builders<Book>.Filter.Lt("Count", 3);
            Collection.DeleteMany(filter);
        }

        public long DeleteAllBooks ()
        {
            var query = Builders<Book>.Filter.Empty;
            return Collection.DeleteMany(query).DeletedCount;
        }
    }
}
