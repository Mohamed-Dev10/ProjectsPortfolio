using System.Collections.Generic;
using System.Linq;

namespace DemoLibrary.Models.Repository
{
    public class AuthorDbRepository : IBookLibrary<Author>
    {
        DemoBooksDbContext ConnectionToDba;

        public AuthorDbRepository(DemoBooksDbContext ConnectionToDba)
        {

            this.ConnectionToDba = ConnectionToDba;

        }

        public void Add(Author author)
        {

            ConnectionToDba.Add(author);
            ConnectionToDba.SaveChanges();
        }

        public Author Find(int idAuthor)
        {

            var Auth = ConnectionToDba.Authors.SingleOrDefault(w => w.OBJECTID == idAuthor);

            return Auth;
        }

        public void Delete(int idAuthor)
        {
            var auth = Find(idAuthor);
            ConnectionToDba.Authors.Remove(auth);
            ConnectionToDba.SaveChanges();
        }

        public void Edit(int id, Author newAuth)
        {
            // var auth = Find(id);

            // auth.NameAuthor = newAuth.NameAuthor;
            ConnectionToDba.Update(newAuth);
            ConnectionToDba.SaveChanges();
        }



        public IList<Author> list()
        {
            return ConnectionToDba.Authors.ToList();
        }

        public int GetLatestObjectId()
        {
            // Assuming you have a database context or repository to work with

            // Retrieve the latest OBJECTID from the database
            var latestAuthor = ConnectionToDba.Authors
                .OrderByDescending(b => b.OBJECTID)
                .FirstOrDefault();

            if (latestAuthor != null)
            {
                return latestAuthor.OBJECTID;
            }

            // If no books exist, return a default value or handle the scenario accordingly
            return 0;

        }
    }
}
