using System.ComponentModel.DataAnnotations;

namespace Task_API.ManualClasses
{
    public class MAdminEditUserTask
    {
        [Required(ErrorMessage = "Title is Required")]
        [MinLength(10, ErrorMessage = "Title must be at least 10 character long")]
        public string? TTitle { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [MinLength(10, ErrorMessage = "Description must be at least 100 character long")]
        public string? TDescription { get; set; }

        [Required]
        public string? TTaskCreater { get; set; }

        [Required(ErrorMessage = "StartDate is Required")]
        public DateTime? TStartDate { get; set; }

        [Required(ErrorMessage = "File is Required")]
        public DateTime? TEndDate { get; set; }

        [Required(ErrorMessage = "File is Required")]
        public byte[]? TFile { get; set; }
    }
}
