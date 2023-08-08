using DemoLibrary.Models;
using DemoLibrary.Models.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BooksLibrary.Models.Repository
{
    public class BookDbRepository : IBookLibrary<Book>
    {

        DemoBooksDbContext ConnectionToDba;




        public BookDbRepository(DemoBooksDbContext ConnectionToDba)
        {

            this.ConnectionToDba = ConnectionToDba;

        }

        public int GetLatestObjectId()
        {
            // Assuming you have a database context or repository to work with

            // Retrieve the latest OBJECTID from the database
            var latestBook = ConnectionToDba.books
                .OrderByDescending(b => b.OBJECTID)
                .FirstOrDefault();

            if (latestBook != null)
            {
                return latestBook.OBJECTID;
            }

            // If no books exist, return a default value or handle the scenario accordingly
            return 0;

        }


        public Book Find(int OBJECTID)
        {
            var book = ConnectionToDba.books.Include(d => d.author).SingleOrDefault(s => s.OBJECTID == OBJECTID);
            return book;
        }

        public void Edit(int OBJECTID, Book Newbook)
        {
            // var book = Find(OBJECTID);

            //book.TitleBook = Newbook.TitleBook;
            //book.DescriptionBook = Newbook.DescriptionBook;
            //book.author = Newbook.author;
            //book.fileUrl = Newbook.fileUrl;
            var existingAuthor = ConnectionToDba.Authors.Find(Newbook.author.OBJECTID);
            existingAuthor.NameAuthor = Newbook.author.NameAuthor;


            ConnectionToDba.Update(Newbook);
            ConnectionToDba.SaveChanges();
        }
        void IBookLibrary<Book>.Add(Book book)
        {
            // book.OBJECTID = ConnectionToDba.books.Max(d => d.OBJECTID) + 1;
            ConnectionToDba.books.Add(book);
            ConnectionToDba.SaveChanges();
        }

        void IBookLibrary<Book>.Delete(int OBJECTID)
        {
            var book = Find(OBJECTID);
            ConnectionToDba.books.Remove(book);
            ConnectionToDba.SaveChanges();
        }


        IList<Book> IBookLibrary<Book>.list()
        {
            return ConnectionToDba.books.ToList();
        }


    }
}
