using LibrarySystem;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class TestsForLibrary
    {
        public Library _catalog;

        [SetUp]
        public void CreateInstances()
        {
            _catalog = new Library();
            Library.libraryBooks = new List<Book>();
            Library.libraryNewspapers = new List<Newspaper>();
            Library.libraryPatents = new List<Patent>();
        }

        [Test]
        public void TestBooksRead()
        {            
            var sr =                       @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<books>" +
                                                    "<book>" +
                                                        @"<name>Me: Elton John Official Autobiography</name>" +
                                                        @"<authors>" +
                                                            @"<author>Elton John</author>" +
                                                        @"</authors>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>Pan MacMillan</publisher>" +
                                                        @"<publicationYear>2019</publicationYear>" +
                                                        @"<pageCount>384</pageCount>" +
                                                        @"<notes>Notes for book</notes>" +
                                                        @"<isbn>978-1-50985-331-1</isbn>" +
                                                    "</book>" +
                                                     "<book>" +
                                                        @"<name>Me: Elton John Official Autobiography</name>" +
                                                        @"<authors>" +
                                                            @"<author>Elton John</author>" +
                                                        @"</authors>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>Pan MacMillan</publisher>" +
                                                        @"<publicationYear>2019</publicationYear>" +
                                                        @"<pageCount>384</pageCount>" +
                                                        @"<notes>Notes for book</notes>" +
                                                        @"<isbn>978-1-50985-331-1</isbn>" +
                                                    "</book>" +
                                                "</books>" +
                                            "</catalog>";

            var books = Library.libraryBooks;
            _catalog.Read(sr);
            var book_act = new List<ICatalogEntity>() { CreateBook(), CreateBook() };

            CollectionAssert.AreEqual(books, book_act, new BooksComparer());
        }

        [Test]
        public void TestNewspaperRead()
        {
            var sr =                       @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<newspapers>" +
                                                    "<newspaper>" +
                                                        @"<name>The Times</name>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>News Corporation</publisher>" +
                                                        @"<publicationYear>2019</publicationYear>" +
                                                        @"<pageCount>28</pageCount>" +
                                                        @"<notes>Notes for newspaper</notes>" +
                                                        @"<number>49</number>" +
                                                        @"<date>" +
                                                            @"<day>15</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</date>" +
                                                        @"<issn>0140-0460</issn>" +
                                                    "</newspaper>" +
                                                "</newspapers>" +
                                            "</catalog>";

            var newspaper = Library.libraryNewspapers;
            _catalog.Read(sr);
            var newspaper_act = new[]
            {
                CreateNewspaper()
            };

            CollectionAssert.AreEqual(newspaper, newspaper_act, new NewspaperComparer());
        }

        [Test]
        public void TestPatentsRead()
        {
            var sr =                       @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<patents>" +
                                                    "<patent>" +
                                                        @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                        @"<inventors>" +
                                                            @"<inventor>Shawn J. BECKER</inventor>" +
                                                            @"<inventor>David L. STEINLAGE</inventor>" +
                                                        @"</inventors>" +
                                                        @"<city>London</city>" +
                                                        @"<registrationNumber>16009291</registrationNumber>" +
                                                        @"<applicationDate>" +
                                                            @"<day>15</day>" +
                                                            @"<month>6</month>" +
                                                            @"<year>2018</year>" +
                                                        @"</applicationDate>" +
                                                        @"<publicationDate>" +
                                                            @"<day>19</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</publicationDate>" +
                                                        @"<pageCount>12</pageCount>" +
                                                        @"<notes>A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.</notes>" +
                                                    "</patent>" +
                                                    "<patent>" +
                                                        @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                        @"<inventors>" +
                                                            @"<inventor>Shawn J. BECKER</inventor>" +
                                                            @"<inventor>David L. STEINLAGE</inventor>" +
                                                        @"</inventors>" +
                                                        @"<city>London</city>" +
                                                        @"<registrationNumber>16009291</registrationNumber>" +
                                                        @"<applicationDate>" +
                                                            @"<day>15</day>" +
                                                            @"<month>6</month>" +
                                                            @"<year>2018</year>" +
                                                        @"</applicationDate>" +
                                                        @"<publicationDate>" +
                                                            @"<day>19</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</publicationDate>" +
                                                        @"<pageCount>12</pageCount>" +
                                                        @"<notes>A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.</notes>" +
                                                    "</patent>" +
                                                "</patents>" +
                                            "</catalog>";

            var patent = Library.libraryPatents;
            _catalog.Read(sr);
            IEnumerable<ICatalogEntity> patent_act = new[]
            {
                CreatePatent(),
                CreatePatent()
            };

            CollectionAssert.AreEqual(patent, patent_act, new PatentsComparer());
        }

        [Test]
        public void TestReadWrite()
        {
            var sr = @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<books>" +
                                                    "<book>" +
                                                        @"<name>Me: Elton John Official Autobiography</name>" +
                                                        @"<authors>" +
                                                        @"<author>Elton John</author>" +
                                                        @"</authors>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>Pan MacMillan</publisher>" +
                                                        @"<publicationYear>2019</publicationYear>" +
                                                        @"<pageCount>384</pageCount>" +
                                                        @"<notes>Notes for book</notes>" +
                                                        @"<isbn>978-1-50985-331-1</isbn>" +
                                                    "</book>" +
                                                     "<book>" +
                                                        @"<name>Me: Elton John Official Autobiography</name>" +
                                                        @"<authors>" +
                                                        @"<author>Elton John</author>" +
                                                        @"</authors>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>Pan MacMillan</publisher>" +
                                                        @"<publicationYear>2019</publicationYear>" +
                                                        @"<pageCount>384</pageCount>" +
                                                        @"<notes>Notes for book</notes>" +
                                                        @"<isbn>978-1-50985-331-1</isbn>" +
                                                    "</book>" +
                                                "</books>" +
                                                "<patents>" +
                                                    "<patent>" +
                                                        @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                        @"<inventors>" +
                                                            @"<inventor>Shawn J. BECKER</inventor>" +
                                                            @"<inventor>David L. STEINLAGE</inventor>" +
                                                        @"</inventors>" +
                                                        @"<city>London</city>" +
                                                        @"<registrationNumber>16009291</registrationNumber>" +
                                                        @"<applicationDate>" +
                                                            @"<day>15</day>" +
                                                            @"<month>6</month>" +
                                                            @"<year>2018</year>" +
                                                        @"</applicationDate>" +
                                                        @"<publicationDate>" +
                                                            @"<day>19</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</publicationDate>" +
                                                        @"<pageCount>12</pageCount>" +
                                                        @"<notes>A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.</notes>" +
                                                    "</patent>" +
                                                "</patents>" +
                                                "<newspapers>" +
                                                    "<newspaper>" +
                                                        @"<name>The Times</name>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>News Corporation</publisher>" +
                                                        @"<pageCount>28</pageCount>" +
                                                        @"<publicationYear>2019</publicationYear>" +                                                        
                                                        @"<notes>Notes for newspaper</notes>" +
                                                        @"<number>49</number>" +
                                                        @"<date>" +
                                                            @"<day>15</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</date>" +
                                                        @"<issn>0140-0460</issn>" +
                                                    "</newspaper>" +
                                                "</newspapers>" +
                                            "</catalog>";
            _catalog.Read(sr);

            var entities = new IEnumerable<ICatalogEntity>[]
            {
                Library.libraryBooks,
                Library.libraryPatents,
                Library.libraryNewspapers                
            };

            var actualResult = _catalog.WriteTo(entities);

            Assert.AreEqual(sr, actualResult.ToString());

        }

        [Test]
        public void TestAllWrite()
        {
            var book = new List<Book>()
            {
                CreateBook(),
                CreateBook()
            };
            var newspaper = new List<Newspaper>()
            {
                CreateNewspaper()
            };
            var patent = new List <Patent>()
            {
                CreatePatent()
            };

            var entities = new IEnumerable<ICatalogEntity>[]
            {
                book,
                newspaper,
                patent
            };
            
            var actualResult = _catalog.WriteTo(entities);

            var expectedResult = @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                            "<books>"+
                                                "<book>" +
                                                    @"<name>Me: Elton John Official Autobiography</name>" +
                                                    @"<authors>"+
                                                    @"<author>Elton John</author>"+
                                                    @"</authors>" +
                                                    @"<publicationPlace>London</publicationPlace>" +
                                                    @"<publisher>Pan MacMillan</publisher>" +
                                                    @"<publicationYear>2019</publicationYear>" +
                                                    @"<pageCount>384</pageCount>" +
                                                    @"<notes>Notes for book</notes>" +
                                                    @"<isbn>978-1-50985-331-1</isbn>" +
                                                "</book>" +
                                                "<book>" +
                                                    @"<name>Me: Elton John Official Autobiography</name>" +
                                                    @"<authors>" +
                                                    @"<author>Elton John</author>" +
                                                    @"</authors>" +
                                                    @"<publicationPlace>London</publicationPlace>" +
                                                    @"<publisher>Pan MacMillan</publisher>" +
                                                    @"<publicationYear>2019</publicationYear>" +
                                                    @"<pageCount>384</pageCount>" +
                                                    @"<notes>Notes for book</notes>" +
                                                    @"<isbn>978-1-50985-331-1</isbn>" +
                                                "</book>" +
                                             "</books>" +
                                             "<newspapers>" +
                                                 "<newspaper>" +
                                                        @"<name>The Times</name>" +
                                                        @"<publicationPlace>London</publicationPlace>" +
                                                        @"<publisher>News Corporation</publisher>" +
                                                        @"<pageCount>28</pageCount>" +
                                                        @"<publicationYear>2019</publicationYear>" +                                                    
                                                        @"<notes>Notes for newspaper</notes>" +
                                                        @"<number>49</number>" +
                                                        @"<date>" +
                                                            @"<day>15</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</date>" +
                                                        @"<issn>0140-0460</issn>" +
                                                    "</newspaper>"+
                                                "</newspapers>" +
                                                "<patents>" +
                                                    "<patent>" +
                                                        @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                        @"<inventors>" +
                                                        @"<inventor>Shawn J. BECKER</inventor>" +
                                                        @"<inventor>David L. STEINLAGE</inventor>" +
                                                        @"</inventors>" +
                                                        @"<city>London</city>" +
                                                        @"<registrationNumber>16009291</registrationNumber>" +
                                                        @"<applicationDate>" +
                                                            @"<day>15</day>" +
                                                            @"<month>6</month>" +
                                                            @"<year>2018</year>" +
                                                        @"</applicationDate>" +
                                                        @"<publicationDate>" +
                                                            @"<day>19</day>" +
                                                            @"<month>12</month>" +
                                                            @"<year>2019</year>" +
                                                        @"</publicationDate>" +
                                                        @"<pageCount>12</pageCount>" +
                                                        @"<notes>A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.</notes>" +
                                                    "</patent>" +
                                                "</patents>" +
                                            "</catalog>";

            Assert.AreEqual(expectedResult, actualResult.ToString());
        }

        class BooksComparer : IComparer, IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.PageCount == y.PageCount
                       && x.Authors.SequenceEqual(y.Authors)
                       && x.Name == y.Name
                       && x.ISBN == y.ISBN
                       && x.Notes == y.Notes
                       && x.PageCount == y.PageCount
                       && x.PublicationYear == y.PublicationYear
                       && x.PublicationPlace == y.PublicationPlace
                       && x.Publisher == y.Publisher? 0 : 1;
            }

            public int Compare(object x, object y)
            {
                return Compare(x as Book, y as Book);
            }
        }

        class NewspaperComparer : IComparer, IComparer<Newspaper>
        {  
            public int Compare(Newspaper x, Newspaper y)
            {
                return x.Name == y.Name &&
                       x.PublicationPlace == y.PublicationPlace &&
                       x.Publisher == y.Publisher &&
                       x.PublicationYear==y.PublicationYear &&
                       x.PageCount == y.PageCount &&
                       x.Notes == y.Notes &&
                       x.Number == y.Number &&
                       x.Date == y.Date &&
                       x.ISSN == y.ISSN ? 0 : 1;
            }
            public int Compare(object x, object y)
            {
                return Compare(x as Newspaper, y as Newspaper);
            }
        }

        class PatentsComparer : IComparer, IComparer<Patent>
        {        
            public int Compare(Patent x, Patent y)
            {
                return x.Name == y.Name &&
                       x.Inventors.SequenceEqual(y.Inventors) &&
                       x.City == y.City &&
                       x.RegistrationNumber == y.RegistrationNumber &&
                       x.ApplicationDate == y.ApplicationDate &&
                       x.PublicationDate == y.PublicationDate &&
                       x.PageCount == y.PageCount &&
                       x.Notes == y.Notes ? 0 : 1;
            }
            public int Compare(object x, object y)
            {
                return Compare(x as Patent, y as Patent);
            }
        }

        private Book CreateBook()
        {
            var book = new Book
            {
                Name = "Me: Elton John Official Autobiography",
                Authors = new List<string>
                {
                    "Elton John"
                },
                PublicationPlace = "London",
                Publisher = "Pan MacMillan",
                PublicationYear = 2019,
                PageCount = 384,
                Notes = "Notes for book",
                ISBN = "978-1-50985-331-1"                            
            };
            return book;
        }

        private Newspaper CreateNewspaper()
        {
            var newspaper = new Newspaper
            {
                Name = "The Times",
                PublicationPlace = "London",
                Publisher = "News Corporation",
                PublicationYear = 2019,
                PageCount = 28,
                Notes = "Notes for newspaper",
                Number = 49,
                Date = new DateTime (2019, 12, 15),
                ISSN = "0140-0460"
            };
            return newspaper;
        }

        private Patent CreatePatent()
        {
            var patent = new Patent
            {
                Name = "MIDDLE BREAKER FOR A TILLAGE IMPLEMENT",
                Inventors = new List<string>
                {
                    "Shawn J. BECKER",
                    "David L. STEINLAGE"
                },
                City = "London",
                RegistrationNumber = 16009291,
                ApplicationDate = new DateTime(2018, 6, 15),
                PublicationDate = new DateTime(2019, 12, 19),
                PageCount  = 12,
                Notes = "A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.",
            };
            return patent;
        }
    }
}