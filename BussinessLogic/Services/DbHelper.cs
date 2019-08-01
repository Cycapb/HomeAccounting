using DomainModels.Exceptions;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BussinessLogic.Services
{
    public class DbHelper : IDbHelper
    {
        private readonly IRepository<PayingItem> _pItemRepo;
        private readonly IRepository<Account> _accRepo;

        public DbHelper(IRepository<PayingItem> pItemRepo, IRepository<Account> accRepo)
        {
            _accRepo = accRepo;
            _pItemRepo = pItemRepo;
        }

        public IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date) && d.UserId == user.Id)
                    .OrderBy(d => d.Date)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetPayingItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(d => d.Date == date.Date && d.UserId == user.Id)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetPayingItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(pItem => pItem.UserId == user.Id && pItem.Date == date.Date)
                        .Select(pItem => new PayItem()
                        {
                            AccountName = pItem.Account.AccountName,
                            CategoryName = pItem.Category.Name,
                            Comment = pItem.Comment,
                            Summ = pItem.Summ,
                            Date = pItem.Date,
                            ItemId = pItem.ItemID
                        })
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetPayingItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(pItem => pItem.UserId == user.Id && (pItem.Date >= dateFrom.Date && pItem.Date <= dateTo.Date))
                        .Select(pItem => new PayItem()
                        {
                            AccountName = pItem.Account.AccountName,
                            CategoryName = pItem.Category.Name,
                            Comment = pItem.Comment,
                            Summ = pItem.Summ,
                            Date = pItem.Date,
                            ItemId = pItem.ItemID,
                            TypeOfFlowId = pItem.Category.TypeOfFlowID
                        })
                    .OrderBy(d => d.Date)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetPayItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(pItem => pItem.CategoryID == categoryId && pItem.UserId == user.Id && (pItem.Date == date.Date))
                        .Select(pItem => new PayItem()
                        {
                            AccountName = pItem.Account.AccountName,
                            CategoryName = pItem.Category.Name,
                            Comment = pItem.Comment,
                            Summ = pItem.Summ,
                            Date = pItem.Date,
                            ItemId = pItem.ItemID
                        })
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetCategoryPayItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId,
            IWorkingUser user)
        {
            try
            {
                return (from pItem in _pItemRepo.GetList(p => p.CategoryID == categoryId && p.UserId == user.Id && (p.Date >= dateFrom.Date) && (p.Date <= dateTo.Date))
                        select new PayItem()
                        {
                            AccountName = pItem.Account.AccountName,
                            CategoryName = pItem.Category.Name,
                            Comment = pItem.Comment,
                            Summ = pItem.Summ,
                            Date = pItem.Date,
                            ItemId = pItem.ItemID
                        })
                    .OrderBy(d => d.Date)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetCategoryPayItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId, IWorkingUser user)
        {
            try
            {
                return _pItemRepo.GetList(pItem => pItem.Category.TypeOfFlowID == typeOfFlowId && pItem.UserId == user.Id && pItem.Date >= dateFrom.Date && pItem.Date <= dateTo.Date)
                        .Select(pItem => new PayItem()
                        {
                            AccountName = pItem.Account.AccountName,
                            CategoryName = pItem.Category.Name,
                            Comment = pItem.Comment,
                            Summ = pItem.Summ,
                            Date = pItem.Date,
                            ItemId = pItem.ItemID
                        })
                    .OrderBy(d => d.Date)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetPayItemsInDatesByTypeOfFlowWeb)} при обращении к БД",
                    e);
            }
        }

        public Task<string> GetBudgetOverAllWeb(IWorkingUser user)
        {
            return Task.Run(() =>
            {
                try
                {
                    return _accRepo.GetList(i => i.UserId == user.Id)                        
                        .Sum(s => s.Cash)
                        .ToString("c");
                }
                catch (DomainModelsException e)
                {
                    throw new ServiceException(
                        $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetBudgetOverAllWeb)} при обращении к БД",
                        e);
                }
            });
        }

        public Task<string> GetBudgetInFactWeb(IWorkingUser user)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        return _accRepo.GetList(b => b.Use && b.UserId == user.Id)                            
                            .Sum(s => s.Cash)
                            .ToString("c");
                    }
                    catch (DomainModelsException e)
                    {
                        throw new ServiceException(
                            $"Ошибка в сервисе {nameof(DbHelper)} в методе {nameof(GetBudgetInFactWeb)} при обращении к БД",
                            e);
                    }
                }
            );
        }

        public decimal GetSummForMonth(List<PayingItem> collection)
        {
            return collection
            .Where(i => i.Date.Month == DateTime.Now.Month && i.Date.Year == DateTime.Now.Year)
            .Sum(i => i.Summ);
        }

        public decimal GetSummForWeek(List<PayingItem> collection)
        {
            var currentDayOfWeek = (int)DateTime.Now.Date.DayOfWeek;
            currentDayOfWeek = currentDayOfWeek == 0 ? 7 : currentDayOfWeek;
            return collection
                .Where(i => DateTime.Now.Date - i.Date <= TimeSpan.FromDays(currentDayOfWeek - 1))
                .Sum(i => i.Summ);
        }

        public decimal GetSummForDay(List<PayingItem> collection)
        {
            return collection
            .Where(i => i.Date == DateTime.Today)
            .Sum(i => i.Summ);
        }
    }
}
