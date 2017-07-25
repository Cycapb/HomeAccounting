using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.Model;
using DomainModels.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class DbHelper:IDbHelper
    {
        private readonly IRepository<PayingItem> _pItemRepo;
        private readonly IRepository<Account> _accRepo;

        public DbHelper(IRepository<PayingItem> pItemRepo, IRepository<Account> accRepo)
        {
            _accRepo = accRepo;
            _pItemRepo = pItemRepo;
        }

        #region Methods for the WEB

        //--------Here are the methods for the WEB--------------------
        public IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {            
            return _pItemRepo.GetList()
                .Where(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date) && d.UserId == user.Id)
                .OrderBy(d => d.Date)
                .ToList();
        }

        public IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, IWorkingUser user)
        {            
            return _pItemRepo.GetList()
                .Where(d => d.Date == date.Date && d.UserId == user.Id)
                .ToList();
        }

        public IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, IWorkingUser user)
        {            
            return (from pItem in _pItemRepo.GetList()
                where pItem.UserId == user.Id
                select new PayItem()
                {
                    AccountName = pItem.Account.AccountName,
                    CategoryName = pItem.Category.Name,
                    Comment = pItem.Comment,
                    Summ = pItem.Summ,
                    Date = pItem.Date,
                    ItemId = pItem.ItemID
                })
                .Where(d => d.Date == date.Date)
                .ToList();                        
        }

        public IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {            
            return (from pItem in _pItemRepo.GetList()
                where pItem.UserId == user.Id
                select new PayItem()
                {
                    AccountName = pItem.Account.AccountName,
                    CategoryName = pItem.Category.Name,
                    Comment = pItem.Comment,
                    Summ = pItem.Summ,
                    Date = pItem.Date,
                    ItemId = pItem.ItemID,
                    TypeOfFlowId = pItem.Category.TypeOfFlowID
                })
                .Where(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date))
                .OrderBy(d => d.Date)
                .ToList();
        }

        public IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, IWorkingUser user)
        {
            return (from pItem in _pItemRepo.GetList()
                where pItem.CategoryID == categoryId && pItem.UserId == user.Id
                select new PayItem()
                {
                    AccountName = pItem.Account.AccountName,
                    CategoryName = pItem.Category.Name,
                    Comment = pItem.Comment,
                    Summ = pItem.Summ,
                    Date = pItem.Date,
                    ItemId = pItem.ItemID
                })
                .Where(d => d.Date == date.Date)
                .ToList();
        }

        public IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId,
            IWorkingUser user)
        {
            return  (from pItem in _pItemRepo.GetList()
                where pItem.CategoryID == categoryId && pItem.UserId == user.Id
                select new PayItem()
                {
                    AccountName = pItem.Account.AccountName,
                    CategoryName = pItem.Category.Name,
                    Comment = pItem.Comment,
                    Summ = pItem.Summ,
                    Date = pItem.Date,
                    ItemId = pItem.ItemID
                })
                .Where(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date))
                .OrderBy(d => d.Date)
                .ToList();
        }

        public IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId, IWorkingUser user)
        {
            return (from pItem in _pItemRepo.GetList()
                where pItem.Category.TypeOfFlowID == typeOfFlowId && pItem.UserId == user.Id
                select new PayItem()
                {
                    AccountName = pItem.Account.AccountName,
                    CategoryName = pItem.Category.Name,
                    Comment = pItem.Comment,
                    Summ = pItem.Summ,
                    Date = pItem.Date,
                    ItemId = pItem.ItemID
                })
                .Where(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date))
                .OrderBy(d => d.Date)
                .ToList();
        }

        public async Task<string> GetBudgetOverAllWeb(IWorkingUser user)
        {
                return (await _accRepo.GetListAsync())
                    .Where(i => i.UserId == user.Id)
                    .Sum(s => s.Cash)
                    .ToString("c");
        }

        public async Task<string> GetBudgetInFactWeb(IWorkingUser user)
        {            
                return (await _accRepo.GetListAsync())
                    .Where(b => b.Use == true && b.UserId == user.Id)
                    .Sum(s => s.Cash)
                    .ToString("c");            
        }

        public decimal GetSummForMonth(List<PayingItem> collection)
        {
            return collection
            .Where(i => i.Date.Month == DateTime.Now.Month && i.Date.Year == DateTime.Now.Year)
            .Sum(i => i.Summ);
        }

        public decimal GetSummForWeek(List<PayingItem> collection)
        {
            return collection
                .Where(i => (DateTime.Now.Date - i.Date) <= TimeSpan.FromDays(7))
                .Sum(i => i.Summ);
        }

        public decimal GetSummForDay(List<PayingItem> collection)
        {
            return collection
            .Where(i => i.Date == DateTime.Today)
            .Sum(i => i.Summ);
        }

        #endregion

    }
}
