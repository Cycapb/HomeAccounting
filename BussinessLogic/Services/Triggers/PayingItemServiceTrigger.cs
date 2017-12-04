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
            try
            {
                if (deletedItem.Category.TypeOfFlowID == 1)
                {
                    deletedItem.Account.Cash -= deletedItem.Summ;
                }
                else
                {
                    deletedItem.Account.Cash += deletedItem.Summ;
                }
                await _accountService.UpdateAsync(deletedItem.Account);
            }
            catch (ServiceException e)
            {
                throw new ServiceException($"Ошибка в сервисе {nameof(PayingItemServiceTrigger)} в методе {nameof(Delete)}", e);
            }
        }

        public async Task Update(PayingItem updatedItem)
        {
            var typeOfFlowId = (await _categoryService.GetItemAsync(updatedItem.CategoryID)).TypeOfFlowID;
            var account = await _accountService.GetItemAsync(updatedItem.AccountID);
        }
    }
}
/*
 				IF (@SummOld>@SummNew)
					BEGIN
						SET @SummTemp=@SummOld-@SummNew
						UPDATE Account SET Cash=@Cash - @SummTemp where Account.AccountID=(select AccountId from inserted)
					END
				ELSE
					BEGIN
						SET @SummTemp=@SummNew-@SummOld
						UPDATE Account SET Cash=@Cash + @SummTemp where Account.AccountID=(select AccountId from inserted)
					END
 */
