﻿using DomainModels.Exceptions;
using DomainModels.Model;
using Services;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Abstract.Helpers;
using WebUI.Core.Models.ReportModels;

namespace WebUI.Core.Implementations.Helpers
{
    public class ReportHelper : IReportHelper
    {
        private readonly IPayingItemService _payingItemService;
        private readonly IAccountService _accountService;
        private bool _disposed;

        public ReportHelper(IPayingItemService payingItemService, IAccountService accountService)
        {
            _accountService = accountService;
            _payingItemService = payingItemService;
        }

        public ReportHelper()
        {
        }

        public IEnumerable<PayingItem> GetPayingItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, string userId)
        {
            try
            {
                return _payingItemService.GetList(d => (d.Date >= dateFrom.Date) && (d.Date <= dateTo.Date) && d.UserId == userId)
                    .OrderBy(d => d.Date)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetPayingItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayingItem> GetPayingItemsByDateWeb(DateTime date, string userId)
        {
            try
            {
                return _payingItemService.GetList(d => d.Date == date.Date && d.UserId == userId)
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetPayingItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsByDateWeb(DateTime date, string userId)
        {
            try
            {
                return _payingItemService.GetList(pItem => pItem.UserId == userId && pItem.Date == date.Date)
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
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetPayingItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, string userId)
        {
            try
            {
                return _payingItemService.GetList(pItem => pItem.UserId == userId && (pItem.Date >= dateFrom.Date && pItem.Date <= dateTo.Date))
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
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetPayItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetCategoryPayItemsByDateWeb(DateTime date, int categoryId, string userId)
        {
            try
            {
                return _payingItemService.GetList(pItem => pItem.CategoryID == categoryId && pItem.UserId == userId && (pItem.Date == date.Date))
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
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetCategoryPayItemsByDateWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetCategoryPayItemsInDatesWeb(DateTime dateFrom, DateTime dateTo, int categoryId, string userId)
        {
            try
            {
                var items = _payingItemService.GetList(p => p.CategoryID == categoryId && p.UserId == userId && (p.Date >= dateFrom.Date) && (p.Date <= dateTo.Date))
                    .Select(pItem => new PayItem()
                    {
                        AccountName = pItem.Account.AccountName,
                        CategoryName = pItem.Category.Name,
                        Comment = pItem.Comment,
                        Summ = pItem.Summ,
                        Date = pItem.Date,
                        ItemId = pItem.ItemID
                    })
                    .OrderBy(d => d.Date);
                return items
                    .ToList();
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetCategoryPayItemsInDatesWeb)} при обращении к БД",
                    e);
            }
        }

        public IEnumerable<PayItem> GetPayItemsInDatesByTypeOfFlowWeb(DateTime dateFrom, DateTime dateTo, int typeOfFlowId, string userId)
        {
            try
            {
                return _payingItemService.GetList(pItem => pItem.Category.TypeOfFlowID == typeOfFlowId && pItem.UserId == userId && pItem.Date >= dateFrom.Date && pItem.Date <= dateTo.Date)
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
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetPayItemsInDatesByTypeOfFlowWeb)} при обращении к БД",
                    e);
            }
        }

        public async Task<string> GetBudgetOverAllAsync(string userId)
        {
            try
            {
                var accounts = await _accountService.GetListAsync(i => i.UserId == userId);

                return accounts.Sum(s => s.Cash).ToString("c");
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetBudgetOverAllAsync)} при обращении к БД",
                    e);
            }
        }

        public async Task<string> GetBudgetInFactAsync(string userId)
        {
            try
            {
                var accounts = await _accountService.GetListAsync(b => b.Use && b.UserId == userId);

                return accounts.Sum(s => s.Cash).ToString("c");
            }
            catch (DomainModelsException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(ReportHelper)} в методе {nameof(GetBudgetInFactAsync)} при обращении к БД",
                    e);
            }
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _accountService.Dispose();
                    _payingItemService.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
