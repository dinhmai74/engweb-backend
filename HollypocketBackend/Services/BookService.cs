using HollypocketBackend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollypocketBackend.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);

            _books = datatbase.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get() => _books.Find(b => true).ToList();

        public Book Get(string id) => _books.Find(b => b.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) => _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Delete(Book bookIn) => _books.DeleteOne(b => b.Id == bookIn.Id);
        public void Delete(string id) => _books.DeleteOne(b => b.Id == id);
    }
}
