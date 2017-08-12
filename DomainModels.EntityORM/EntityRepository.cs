using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;

namespace DomainModels.EntityORM
{
    public class EntityRepository<T>:IRepository<T> where T:class
    {
        private readonly DbSet<T> _dbSet;
        private readonly AccountingContext _context;

        public EntityRepository(AccountingContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public EntityRepository()
        {
            _context = new AccountingContext();
            _dbSet = _context.Set<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public IEnumerable<T> GetList()
        {
            return _dbSet;
        }

        public virtual Task<IEnumerable<T>> GetListAsync()
        {
            return Task.Run(() =>  _dbSet.AsEnumerable());
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public virtual Task<T> CreateAsync(T item)
        {
            return Task.Run((() => _dbSet.Add(item)));
        }

        public void Delete(int id)
        {
            T item = _dbSet.Find(id);
            if (item != null)
            {
                _dbSet.Remove(item);
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            T item = await _dbSet.FindAsync(id);
            if (item != null)
            {
                await Task.Run((() => _dbSet.Remove(item)));
            }
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public virtual Task UpdateAsync(T item)
        {
            return Task.Run((() =>
            {
                _context.Entry(item).State = EntityState.Modified;
            }));
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return Task.Run(() => _context.SaveChanges());
        }

        public Task SaveAsync(IProgress<string> onComplete)
        {
            return Task.Run(() =>
            {
                _context.SaveChanges();
                onComplete.Report("Данные сохранены в базе");
           });
        }

        public T GetItem(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetItemAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
