using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model
{
    [Table(nameof(TodoGroup))]
    public class TodoGroup
    {
        public TodoGroup()
        {
            TodoItems = new HashSet<TodoItem>();
        }

        public int Id { get; set; }
                
        [StringLength(50)]
        [Required(ErrorMessage = "Не задано имя группы")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не задан пользователь")]
        public string UserId { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
