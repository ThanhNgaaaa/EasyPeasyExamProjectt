using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public decimal? RatingStar { get; set; }

    public virtual User? User { get; set; }
}
