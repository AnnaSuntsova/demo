using LibrarySystem;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarySystem.Entities;

namespace Tests
{
    public class TestsForLibrary
    {
        private Lybrary _catalog;

        [Test]
        public void TestBooksRead()
        {
            _catalog = new Lybrary();
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<book>"+
                                                    @"<name>Me: Elton John Official Autobiography</name>" +
                                                    @"<authors>" +
                                                    @"<author>Elton John</author>" +
                                                    @"</authors>" +
                                                    @"<publicationPlace>London</publicationPlace>" +
                                                    @"<publisher>Pan MacMillan</publisher>" +
                                                    @"<publicationYear>2019</publicationYear>" +
                                                    @"<pageCount>384</pageCount>" +
                                                    @"<notes>Notes for book</notes>" +
                                                    @"<ISBN>978-1-50985-331-1</ISBN>" +
                                                "</book>" +
                                            "</catalog>");

            IEnumerable<ICatalogEntity> books = _catalog.Read(sr).ToList();
            IEnumerable<ICatalogEntity> book_act = new[]
            {
                CreateBook()
            };

            CollectionAssert.AreEqual(books, book_act, new BooksComparer());

            sr.Dispose();
        }

        [Test]
        public void TestNewspaperRead()
        {
            _catalog = new Lybrary();
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
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
                                                    @"<ISSN>0140-0460</ISSN>" +
                                                "</newspaper>" +
                                            "</catalog>");

            IEnumerable<ICatalogEntity> newspaper = _catalog.Read(sr).ToList();
            IEnumerable<ICatalogEntity> newspaper_act = new[]
            {
                CreateNewspaper()
            };

            CollectionAssert.AreEqual(newspaper, newspaper_act, new NewspaperComparer());

            sr.Dispose();
        }

        [Test]
        public void TestPatentsRead()
        {
            _catalog = new Lybrary();
            TextReader sr = new StringReader(@"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
                                                "<patent>" +
                                                    @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                    @"<inventors>" +
                                                        @"<inventor>Shawn J. BECKER</inventor>" +
                                                        @"<inventor>David L. STEINLAGE</inventor>" +
                                                    @"</inventors>" +
                                                    @"<city>London</city>" +
                                                    @"<registrationNumber>16009291</registrationNumber>" +
                                                    @"<applicationDate>2018-06-15</applicationDate>" +
                                                        @"<day>15</day>" +
                                                        @"<month>06</month>" +
                                                        @"<year>2018</year>" +
                                                    @"</applicationDate>" +
                                                    @"<publicationDate>" +
                                                        @"<day>19</day>" +
                                                        @"<month>12</month>" +
                                                        @"<year>2019</year>" +
                                                    @"</publicationDate>" +
                                                    @"<pageCount>12</pageCount>" +
                                                    @"<notes>A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.</notes>" +
                                                    @"<ISBN>9781509853311</ISBN>" +
                                                "</patent>" +
                                            "</catalog>");

            IEnumerable<ICatalogEntity> patent = _catalog.Read(sr).ToList();
            IEnumerable<ICatalogEntity> patent_act = new[]
            {
                CreatePatent()
            };

            CollectionAssert.AreEqual(patent, patent_act, new PatentsComparer());

            sr.Dispose();
        }

        [Test]
        public void TestAllWrite()
        {
            _catalog = new Lybrary();
            var book = CreateBook();
            var newspaper = CreateNewspaper();
            var patent = CreatePatent();

            var entities = new ICatalogEntity[]
            {
                book,
                newspaper,
                patent
            };

            var actualResult = new StringWriter();

            _catalog.WriteTo(actualResult, entities);

            var expectedResult = @"<?xml version=""1.0"" encoding=""utf-16""?>" +
                                            "<catalog>" +
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
                                                    @"<ISBN>978-1-50985-331-1</ISBN>" +
                                                "</book>" +
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
                                                    @"<ISSN>0140-0460</ISSN>" +
                                                "</newspaper>" +
                                                "<patent>" +
                                                    @"<name>MIDDLE BREAKER FOR A TILLAGE IMPLEMENT</name>" +
                                                    @"<inventors>" +
                                                    @"<inventor>Shawn J. BECKER</inventor>" +
                                                    @"<inventor>David L. STEINLAGE</inventor>" +
                                                    @"</inventors>" +
                                                    @"<city>London</city>" +
                                                    @"<registrationNumber>16009291</registrationNumber>" +
                                                    @"<applicationDate>2018-06-15</applicationDate>" +
                                                        @"<day>15</day>" +
                                                        @"<month>06</month>" +
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
                                            "</catalog>";
            var str = actualResult.ToString();
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
                       x.Date.Day == y.Date.Day &&
                       x.Date.Month == y.Date.Month &&
                       x.Date.Year == y.Date.Year &&
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
                Date = new Date
                {
                    Day = 15,
                    Month = 12,
                    Year = 2019
                },
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
                City  = "London",
                RegistrationNumber   = 16009291,
                ApplicationDate  = DateTime.Parse("15/06/2018"),
                PublicationDate = DateTime.Parse("19/12/2019"),
                PageCount  = 12,
                Notes = "A tillage implement having a frame member extending in a fore-aft direction of the implement, the frame member pivotally connected in a foldable configuration, the frame member comprising a main frame section.",
            };
            return patent;
        }
    }
}