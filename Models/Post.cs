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

    public string StudentGender { get; set; } = null!;

    public decimal? Fee { get; set; }

    public string? TypeOfFee { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public bool IsHidden { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StudyTime> StudyTimes { get; set; } = new List<StudyTime>();

    public virtual User User { get; set; } = null!;
}
