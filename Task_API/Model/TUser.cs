using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task_API.Model;

public partial class TUser
{
    public int UId { get; set; }

    [Required(ErrorMessage = "Name is Required")]
    [MinLength(10, ErrorMessage = "Name must be at least 10 character long")]
    public string? UName { get; set; }

    [Required(ErrorMessage = "UserName is Required")]
    [MinLength(10, ErrorMessage = "AName must be at least 10 character long")]
    public string? UUserName { get; set; }

    [Required(ErrorMessage = "Password is Required")]
    [EmailAddress(ErrorMessage = "Provide a propper Email address")]
    public string? UPassword { get; set; }

    [Required(ErrorMessage = "Roles is Required")]
    public string? Roles { get; set; }

    [Required(ErrorMessage = "Email is Required")]
    public string? UEmail { get; set; }

    [Required(ErrorMessage = "ActiveStatus is Required")]
    public string? ActiveStatus { get; set; }
}