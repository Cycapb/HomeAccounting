using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using DomainModels.EntityORM;
using HomeAccountingSystem_DAL.Model;

namespace HomeAccountingSystem_UnitTests
{
    public class EntityRepositoryFake<T> : EntityRepository<T> where T : class
    {
        private readonly AccountingContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepositoryFake() { }

        public EntityRepositoryFake(AccountingContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void UpdateItem(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public override Task<IEnumerable<T>> GetListAsync()
        {
            return Task.Run(() =>
            {
                IEnumerable<T> list = _dbSet;
                return list;
            });
        }

        public override Task<T> GetItemAsync(int id)
        {
            return Task.Run(() => _dbSet.Find(id));
        }

        public override async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public override Task UpdateAsync(T item)
        {
            UpdateItem(item);
            return Task.Run(() => item);
        }

        public override Task<T> CreateAsync(T item)
        {
            return Task.Run(() => _dbSet.Add(item));
        }

        public override Task DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                var item = _dbSet.Find(id);
                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            });
        }
    }
}
