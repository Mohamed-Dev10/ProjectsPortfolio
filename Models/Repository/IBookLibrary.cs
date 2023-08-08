using System.Collections.Generic;

namespace DemoLibrary.Models.Repository
{
    public interface IBookLibrary<TEntity>
    {

        public IList<TEntity> list();
        public TEntity Find(int id);
        public void Add(TEntity entity);
        public void Edit(int id, TEntity entity);
        public void Delete(int OBJECTID);
        public int GetLatestObjectId();




    }
}
