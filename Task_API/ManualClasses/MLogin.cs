using System.ComponentModel.DataAnnotations;

namespace Task_API.ManualClasses
{
    public class MLogin
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UUserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? UPassword { get; set; }
    }
}
