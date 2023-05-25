using System.ComponentModel.DataAnnotations;

namespace Task_API.ManualClasses
{
    public class MUser
    {
        [Required(ErrorMessage = "Name is Required")]
        [MinLength(10, ErrorMessage = "Name must be at least 10 character long")]
        public string? UName { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        [MinLength(8, ErrorMessage = "AName must be at least 10 character long")]
        public string? UUserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 character long")]
        public string? UPassword { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Provide a propper Email address")]
        public string? UEmail { get; set; }
    }
}