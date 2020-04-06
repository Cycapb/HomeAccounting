using DomainModels.Model;
using Microsoft.EntityFrameworkCore;

namespace DomainModels.EntityORM.Core.Infrastructure
{
    public class ToDoContextCore : DbContext
    {
        public ToDoContextCore(DbContextOptions<ToDoContextCore> dbContextOptions) : base(dbContextOptions)
        {

        }

        public virtual DbSet<TodoGroup> TodoGroups { get; set; }

        public virtual DbSet<TodoItem> TodoItems { get; set; }
    }
}
