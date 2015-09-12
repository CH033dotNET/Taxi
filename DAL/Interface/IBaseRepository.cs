using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        System.Linq.IQueryable<TEntity> All { get; }
        System.Collections.Generic.IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null,
            Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void SetStateModified(TEntity entity);
    }
}
