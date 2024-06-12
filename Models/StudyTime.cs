using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class StudyTime
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public TimeOnly? From { get; set; }

    public TimeOnly? To { get; set; }

    public int? DayOfWeek { get; set; }

    public virtual Post Post { get; set; } = null!;
}
