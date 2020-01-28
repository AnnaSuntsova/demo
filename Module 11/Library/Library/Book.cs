using MongoDB.Bson;

namespace IntroToNoSQL
{
    public class Book
    {
       public ObjectId Id { get; set; }
       public string Name { get; set; }
       public string Author { get; set; }
       public int Count { get; set; }
       public string [] Genre { get; set; }
       public int Year { get; set; }

        public override string ToString()
        {
            var showAuthor = string.IsNullOrWhiteSpace(Author) ? "-":  Author.ToString();
            string genreString="";
            foreach (var genre in Genre)
                genreString += genre + " ";
            return $"Name: {Name.ToString()}, author: {showAuthor}, count: {Count.ToString()}, genre: {genreString.Trim()}, year: {Year.ToString()}";
        }
    }
}
