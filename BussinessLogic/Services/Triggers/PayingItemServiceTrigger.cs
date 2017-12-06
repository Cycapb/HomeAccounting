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
                    $"Ошибка {e.GetType()} в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(Update)}", e);
            }
            catch (NullReferenceException e)
            {
                throw new ServiceException($"Ошибка {e.GetType()} в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(Insert)}", e);
            }
        }

        private async Task UpdateIncome(PayingItem oldItem, PayingItem newItem)
        {
            if (oldItem.Summ > newItem.Summ)
            {
                newItem.Account.Cash -= oldItem.Summ - newItem.Summ;
                await _accountService.UpdateAsync(newItem.Account);
            }
            else
            {
                newItem.Account.Cash += newItem.Summ - oldItem.Summ;
                await _accountService.UpdateAsync(newItem.Account);
            }
        }

        private async Task UpdateOutgo(PayingItem oldItem, PayingItem newItem)
        {
            if (oldItem.Summ > newItem.Summ)
            {
                newItem.Account.Cash += oldItem.Summ - newItem.Summ;
                await _accountService.UpdateAsync(newItem.Account);
            }
            else
            {
                newItem.Account.Cash -= newItem.Summ - oldItem.Summ;
                await _accountService.UpdateAsync(newItem.Account);
            }
        }
    }
}
/*
 	BEGIN
		set @AccountIdNew=(select AccountId from inserted)
		set @AccountIdOld=(select AccountId from deleted)		
		set @CashOld=(select Cash from Account where AccountID=@AccountIdOld)		

		IF (@AccountIdOld<>@AccountIdNew)
			IF(@ClosedPeriod)=0
			BEGIN
				IF (select c.TypeOfFlowID from inserted as i 
					join Category as c on c.CategoryID=i.CategoryID)=1
				BEGIN
					UPDATE Account SET Cash=@Cash + @SummNewForUpdate where Account.AccountID=@AccountIdNew
					UPDATE Account SET Cash=@CashOld - @SummOld where Account.AccountID=@AccountIdOld
				END
				ELSE
				BEGIN
					UPDATE Account SET Cash=@Cash - @SummNewForUpdate where Account.AccountID=@AccountIdNew
					UPDATE Account SET Cash=@CashOld + @SummOld where Account.AccountID=@AccountIdOld
				END
			END
	END
 */
