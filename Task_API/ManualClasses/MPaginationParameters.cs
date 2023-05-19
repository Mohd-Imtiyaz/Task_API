using System.ComponentModel.DataAnnotations;
using Task_API.Model;

namespace Task_API.ManualClasses
{
    public class MPaginationParameters
    {
        public List<TUserTask> AllTasks { get; set; } = new List<TUserTask>();

        [Required(ErrorMessage = "Pages is Required")]
        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}