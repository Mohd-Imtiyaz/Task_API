using System;
using System.Collections.Generic;

namespace Task_API.Model;

public partial class TUserTask
{
    public int TId { get; set; }

    public string? TTitle { get; set; }

    public string? TDescription { get; set; }

    public string? TTaskCreater { get; set; }

    public DateTime? TStartDate { get; set; }

    public DateTime? TEndDate { get; set; }

    public byte[]? TFile { get; set; }
}
