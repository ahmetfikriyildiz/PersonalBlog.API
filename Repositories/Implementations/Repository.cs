using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly PersonalBlogDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(PersonalBlogDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
