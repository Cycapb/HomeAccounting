using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using DomainModels.Exceptions;

namespace DomainModels.EntityORM
{
    public class EntityRepository<T> : IRepository<T> where T : class
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
            try
            {
                return _dbSet;
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetList)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public virtual Task<IEnumerable<T>> GetListAsync()
        {
            try
            {
                return Task.Run(() => _dbSet.AsEnumerable());
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetListAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public void Create(T item)
        {
            try
            {
                _dbSet.Add(item);
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Create)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public virtual Task<T> CreateAsync(T item)
        {
            try
            {
                return Task.Run(() => _dbSet.Add(item));
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(CreateAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public void Delete(int id)
        {
            try
            {
                T item = _dbSet.Find(id);
                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Delete)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                T item = await _dbSet.FindAsync(id);
                if (item != null)
                {
                    await Task.Run((() => _dbSet.Remove(item)));
                }
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(DeleteAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public void Update(T item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Update)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public virtual Task UpdateAsync(T item)
        {
            try
            {
                return Task.Run(() =>
                {
                    _context.Entry(item).State = EntityState.Modified;
                });
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(UpdateAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }

        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Save)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return Task.Run(() => _context.SaveChanges());
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(SaveAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public Task SaveAsync(IProgress<string> onComplete)
        {
            try
            {
                return Task.Run(() =>
                {
                    _context.SaveChanges();
                    onComplete.Report("Данные сохранены в базе");
                });
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(SaveAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public T GetItem(int id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetItem)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }

        public virtual async Task<T> GetItemAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetItemAsync)} репозитория {nameof(EntityRepository<T>)}", ex);
            }
        }
    }
}
