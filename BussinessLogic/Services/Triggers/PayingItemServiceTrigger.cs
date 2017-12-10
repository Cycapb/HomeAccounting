using System;
using System.Threading.Tasks;
using DomainModels.Model;
using Services;
using Services.Exceptions;
using Services.Triggers;

namespace BussinessLogic.Services.Triggers
{
    public class PayingItemServiceTrigger :IServiceTrigger<PayingItem>
    {
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public PayingItemServiceTrigger(ICategoryService categoryService, IAccountService accountService)
        {
            _categoryService = categoryService;
            _accountService = accountService;
        }

        public async Task Insert(PayingItem insertedItem)
        {
            if (insertedItem == null){ return; }

            try
            {
                var typeOfFlowId = (await _categoryService.GetItemAsync(insertedItem.CategoryID)).TypeOfFlowID;
                var account = await _accountService.GetItemAsync(insertedItem.AccountID);
                if (typeOfFlowId == 1)
                {
                    account.Cash += insertedItem.Summ;
                }
                else
                {
                    account.Cash -= insertedItem.Summ;
                }
                await _accountService.UpdateAsync(account);
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(Insert)}", e);
            }
        }

        public async Task Delete(PayingItem deletedItem)
        {
            if (deletedItem == null) { return; }

            try
            {
                var typeOfFlowId = (await _categoryService.GetItemAsync(deletedItem.CategoryID)).TypeOfFlowID;
                var accountToUpdate = await _accountService.GetItemAsync(deletedItem.AccountID);
                if (typeOfFlowId == 1)
                {
                    accountToUpdate.Cash -= deletedItem.Summ;
                }
                else
                {
                    accountToUpdate.Cash += deletedItem.Summ;
                }
                await _accountService.UpdateAsync(accountToUpdate);
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(Delete)}", e);
            }
        }

        public async Task Update(PayingItem oldItem, PayingItem newItem)
        {
            if (SumAndAccountHasChanged(oldItem,newItem) || AccountHasChanged(oldItem, newItem))
            {
                await UpdateAccountsAndSum(oldItem, newItem);
                return;
            } 

            if (SumHasChanged(oldItem, newItem))
            {
                await UpdateAccountSum(oldItem, newItem);
            }
        }

        private bool AccountHasChanged(PayingItem oldItem, PayingItem newItem)
        {
            return oldItem.AccountID != newItem.AccountID;
        }

        private async Task UpdateAccountsAndSum(PayingItem oldItem, PayingItem newItem)
        {
            if (oldItem.AccountID != newItem.AccountID)
            {
                var oldAccount = await _accountService.GetItemAsync(oldItem.AccountID);
                var newAccount = await _accountService.GetItemAsync(newItem.AccountID);
                switch (newItem.Category.TypeOfFlowID)
                {
                    case 1:
                        newAccount.Cash += newItem.Summ;
                        oldAccount.Cash -= oldItem.Summ;
                        await _accountService.UpdateAsync(oldAccount);
                        await _accountService.UpdateAsync(newAccount);
                        break;
                    case 2:
                        newAccount.Cash -= newItem.Summ;
                        oldAccount.Cash += oldItem.Summ;
                        await _accountService.UpdateAsync(oldAccount);
                        await _accountService.UpdateAsync(newAccount);
                        break;
                }
            }
        }

        private async Task UpdateAccountSum(PayingItem oldItem, PayingItem newItem)
        {
            try
            {
                switch (newItem.Category.TypeOfFlowID)
                {
                    case 1:
                        await UpdateIncome(oldItem, newItem);
                        break;
                    case 2:
                        await UpdateOutgo(oldItem, newItem);
                        break;
                }
            }
            catch (ServiceException e)
            {
                throw new ServiceException(
                    $"Ошибка в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(UpdateAccountSum)}", e);
            }
            catch (NullReferenceException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(UpdateAccountSum)}", e);
            }
        }

        private bool SumHasChanged(PayingItem oldItem, PayingItem newItem)
        {
            return oldItem.Summ != newItem.Summ;
        }

        private bool SumAndAccountHasChanged(PayingItem oldItem, PayingItem newItem)
        {
            return oldItem.Summ != newItem.Summ && oldItem.AccountID != newItem.AccountID;
        }

        private async Task UpdateIncome(PayingItem oldItem, PayingItem newItem)
        {
            var accountToUpdate = await _accountService.GetItemAsync(newItem.AccountID);
            if (oldItem.Summ > newItem.Summ)
            {
                accountToUpdate.Cash -= oldItem.Summ - newItem.Summ;
                await _accountService.UpdateAsync(accountToUpdate);
            }
            else
            {
                accountToUpdate.Cash += newItem.Summ - oldItem.Summ;
                await _accountService.UpdateAsync(accountToUpdate);
            }
        }

        private async Task UpdateOutgo(PayingItem oldItem, PayingItem newItem)
        {
            var accountToUpdate = await _accountService.GetItemAsync(newItem.AccountID);
            if (oldItem.Summ > newItem.Summ)
            {
                accountToUpdate.Cash += oldItem.Summ - newItem.Summ;
                await _accountService.UpdateAsync(accountToUpdate);
            }
            else
            {
                accountToUpdate.Cash -= newItem.Summ - oldItem.Summ;
                await _accountService.UpdateAsync(accountToUpdate);
            }
        }
    }
}