namespace DomainModels.Model
{
    public interface IWorkingUser
    {
        string Name { get; set; }

        string Id { get; set; }

        string Email { get; set; }
    }
}
