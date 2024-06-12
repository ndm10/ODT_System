using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Time { get; set; }

    public bool IsDelete { get; set; }

    public virtual User FromNavigation { get; set; } = null!;

    public virtual User ToNavigation { get; set; } = null!;
}
