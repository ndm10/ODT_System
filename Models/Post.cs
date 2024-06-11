using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class Post
{
    public int Id { get; set; }

    public string ContactPhone { get; set; } = null!;

    public string PostContent { get; set; } = null!;

    public string? Address { get; set; }

    public int? NumberOfStudent { get; set; }

    public byte? StudentGender { get; set; }

    public decimal? Fee { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
