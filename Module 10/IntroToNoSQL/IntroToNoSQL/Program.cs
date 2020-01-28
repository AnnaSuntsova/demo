using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntroToNoSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookQueries = new BooksQueries();
            bookQueries.SetConnection();

            var books = new List<Book>();

            var hobbit = new Book
            {
                Name = "Hobbit",
                Author = "Tolkien",
                Count = 5,
                Year = 2014,
            };
            hobbit.Genre = new [] { "fantasy"};
            books.Add(hobbit);

            var lord = new Book()
            {
                Name = "Lord of the ring",
                Author = "Tolkien",
                Count = 3,                                
                Year = 2015
            };
            lord.Genre = new[] { "fantasy" };
            books.Add(lord);

            var kolobok = new Book()
            {
                Name = "Kolobok",
                Count = 10,
                Year = 2001
            };
            kolobok.Genre = new[] { "kids" };
            books.Add(kolobok);

            var repka = new Book()
            {
                Name = "Repka",
                Count = 11,
                Year = 2000
            };
            repka.Genre = new[] { "kids" };
            books.Add(repka);

            var stiopa = new Book()
            {
                Name = "Dyadya Stiopa",
                Author = "Mihalkov",
                Count = 1,
                Year = 2001
            };
            stiopa.Genre = new[] { "kids" };
            books.Add(stiopa);

            bookQueries.Add(books);


            var bookResult = bookQueries.Find3BooksWithCountGreater1();
            foreach (var item in bookResult)
            {
                Console.WriteLine(item);
            }
            
            var booksAmount = bookQueries.FindBooksAmountWithCountGreater1();
            Console.WriteLine($"Books amount with count > 1: {booksAmount}");

            Console.WriteLine("Book with max count:");
            var bookMaxCount = bookQueries.FindBookWithMaxCount();
            Console.WriteLine(bookMaxCount.ToString());

            Console.WriteLine("Book with min count:");
            var bookMinCount = bookQueries.FindBookWithMinCount();
            Console.WriteLine(bookMinCount.ToString());

            Console.WriteLine("Authors:");
            var authorRes = bookQueries.GetAuthors();
            foreach (var author in authorRes)
            {
                Console.WriteLine(author);
            }

            Console.WriteLine("Books without authors:");
            var booksResult = bookQueries.GetBooksWithoutAuthors();
            foreach (var book in booksResult)
            {
                Console.WriteLine(book.ToString());
            }

            Console.WriteLine("Books list after count updating:");
            bookQueries.IncreaseCount();
            var bookList = bookQueries.Collection.Find(book => true).ToList();
            foreach (var book in bookList)
            {
                Console.WriteLine(book.ToString());
            }

            Console.WriteLine("Books list after genre updating:");
            bookQueries.AddGenre();
            var bookGenreList = bookQueries.Collection.Find(book => true).ToList();
            foreach (var book in bookList)
            {
                Console.WriteLine(book.ToString());
            }

            Console.WriteLine("Books list after deleting of books with count < 3:");
            bookQueries.DeleteBooksWithCountLower3();
            var bookListAfterDeleting = bookQueries.Collection.Find(book => true).ToList();
            foreach (var book in bookListAfterDeleting)
            {
                Console.WriteLine(book.ToString());
            }

            Console.WriteLine($"Delete books.\n Count of deleted rows: {bookQueries.DeleteAllBooks()}");            

            Console.ReadKey();
        }
    }
}
