using System;
using System.Collections.Generic;

namespace ODT_System.Models;

public partial class Post
{
    public int Id { get; set; }

    public string ContactPhone { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string StudyAddress { get; set; } = null!;

    public int NumberOfStudent { get; set; }

    public DateOnly StartDate { get; set; }

    public decimal? StudyHour { get; set; }

    public string Subject { get; set; } = null!;

    public byte StudentGender { get; set; }

    public decimal? Fee { get; set; }

    public int? TypeOfFee { get; set; }

    public int? DayPerWeek { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<StudyTime> StudyTimes { get; set; } = new List<StudyTime>();

    public virtual User User { get; set; } = null!;
}
