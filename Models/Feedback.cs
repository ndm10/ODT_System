using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public int FeedbeckToId { get; set; }

    public string Status { get; set; } = null!;

    public bool IsDelete { get; set; }

    public int CreateById { get; set; }

    public virtual User CreateBy { get; set; } = null!;

    public virtual User FeedbeckTo { get; set; } = null!;
}
