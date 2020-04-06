using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Model
{
    [Table(nameof(TodoItem))]
    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "У задачи должно быть описание")]
        [StringLength(250)]
        public string Description { get; set; }

        public int GroupId { get; set; }

        public bool IsFinished { get; set; }

        [Required(ErrorMessage = "Не задан пользователь")]
        public string UserId { get; set; }

        public virtual TodoGroup Group { get; set; }
    }
}
