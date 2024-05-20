using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool Gender { get; set; }

    public DateOnly Dob { get; set; }

    public string Phone { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Desciption { get; set; }
}
