namespace Task_API.ManualClasses
{
    public class MAddingTask
    {
        public string? TTitle { get; set; }

        public string? TDescription { get; set; }

        public string? TTaskCreater { get; set; }

        public DateTime? TStartDate { get; set; }

        public DateTime? TEndDate { get; set; }

        public byte[]? TFile { get; set; }
    }
}
