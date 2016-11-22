namespace HomeAccountingSystem_DAL.Abstract
{
    public interface IWorkingUser
    {
        string Name { get; set; }
        string Password { get; set; }
        string Id { get; set; }
        string Email { get; set; }
    }
}
