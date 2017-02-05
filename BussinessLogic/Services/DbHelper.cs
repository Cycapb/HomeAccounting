using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModels.EntityORM;
using HomeAccountingSystem_DAL.Abstract;
using DomainModels.Model;
using HomeAccountingSystem_DAL.Repositories;
using Services;

namespace BussinessLogic.Services
{
    public class DbHelper:IDbHelper
    {
        #region Methods for the WEB

        //--------Here are the methods for the WEB--------------------
        public IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {
            IRepository<PayingItem> pRepository = new EntityRepository<PayingItem>();
            return pRepository.GetList()
                .Where(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date) && d.UserId == user.Id)
                .OrderBy(d => d.Date)
                .ToList();
        }

        public IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, IWorkingUser user)
        {
            IRepository<PayingItem> pRepository = new EntityRepository<PayingItem>();
            return pRepository.GetList()
                .Where(d => d.Date == date.Date && d.UserId == user.Id)
                .ToList();
        }

        public IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, IWorkingUser user)
        {
            var context = new AccountingContext();
            var list = (from pItem in context.PayingItem
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
            context.Dispose();
            return list;
        }

        public IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, IWorkingUser user)
        {
            var context = new AccountingContext();
            var list = (from pItem in context.PayingItem
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
            context.Dispose();
            return list;
        }

        public IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, IWorkingUser user)
        {
            var context = new AccountingContext();
            var list = (from pItem in context.PayingItem
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
            context.Dispose();
            return list;
        }

        public IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId,
            IWorkingUser user)
        {
            var context = new AccountingContext();
            var list = (from pItem in context.PayingItem
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
            context.Dispose();
            return list;
        }

        public IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo,
            int typeOfFlowId, IWorkingUser user)
        {
            var context = new AccountingContext();
            var list = (from pItem in context.PayingItem
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
            context.Dispose();
            return list;
        }

        public Task<string> GetBudgetOverAllWeb(IWorkingUser user)
        {
            IRepository<Account> accRepository = new EntityRepository<Account>();
            return Task.Run((() =>
            {
                return accRepository
                    .GetList()
                    .Where(i => i.UserId == user.Id)
                    .Sum(s => s.Cash)
                    .ToString("c");
            }));
        }

        public Task<string> GetBudgetInFactWeb(IWorkingUser user)
        {
            IRepository<Account> accRepository = new EntityRepository<Account>();
            return Task.Run((() =>
            {
                return accRepository
                    .GetList()
                    .Where(b => b.Use == true && b.UserId == user.Id)
                    .Sum(s => s.Cash)
                    .ToString("c");
            }));
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
