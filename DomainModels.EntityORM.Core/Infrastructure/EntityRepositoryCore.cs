using DomainModels.Exceptions;
using DomainModels.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainModels.EntityORM.Core.Infrastructure
{
    public class EntityRepositoryCore<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext
    {
        private readonly DbSet<T> _dbSet;
        private readonly TContext _context;

        public EntityRepositoryCore(TContext context)
        {
            _context = context;
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

        public virtual IEnumerable<T> GetList()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetList)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetListAsync)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }


        public virtual T Create(T item)
        {
            try
            {
                return _dbSet.Add(item).Entity;
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Create)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }

        }

        public virtual void Delete(int id)
        {
            try
            {
                var item = _dbSet.Find(id);

                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Delete)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }

        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var item = await _dbSet.FindAsync(id);

                if (item != null)
                {
                    _dbSet.Remove(item);
                }
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(DeleteAsync)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }

        }

        public virtual void Update(T item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Update)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }

        }

        public virtual void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(Save)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }

        public virtual async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(SaveAsync)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }

        public virtual T GetItem(int id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetItem)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
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
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetItemAsync)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetList)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DomainModelsException($"Возникла ошибка на уровне доступа к данным в методе {nameof(GetListAsync)} репозитория {nameof(EntityRepositoryCore<T, TContext>)}", ex);
            }
        }
    }
}
