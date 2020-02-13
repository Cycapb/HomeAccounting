namespace DomainModels.EntityORM.Infrastructure
{
    public class AccountingContextCore : AccountingContext
    {
        public AccountingContextCore() : base()
        {
        }

        public AccountingContextCore(string name) : base(name)
        {

        }
    }
}
