namespace Task_API.ManualClasses
{
    public class MAddTask
    {
        public string? TTitle { get; set; }

        public string? TDescription { get; set; }

        public DateTime? TStartDate { get; set; }

        public DateTime? TEndDate { get; set; }

        public byte[]? TFile { get; set; }
    }
}
