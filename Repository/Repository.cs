using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _uof;

        public Repository(AppDbContext contexto)
        {
            _uof = contexto;
        }

        public IQueryable<T> Get()
        {
            return _uof.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _uof.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public void Add(T entity)
        {
            _uof.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _uof.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _uof.Entry(entity).State= EntityState.Modified;
            _uof.Set<T>().Update(entity);
        }
    }
}
