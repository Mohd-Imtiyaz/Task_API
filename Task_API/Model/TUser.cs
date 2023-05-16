using System;
using System.Collections.Generic;

namespace Task_API.Model;

public partial class TUser
{
    public int UId { get; set; }

    public string? UName { get; set; }

    public string? UUserName { get; set; }

    public string? UPassword { get; set; }

    public string? Roles { get; set; }

    public string? UEmail { get; set; }

    public string? ActiveStatus { get; set; }
}
