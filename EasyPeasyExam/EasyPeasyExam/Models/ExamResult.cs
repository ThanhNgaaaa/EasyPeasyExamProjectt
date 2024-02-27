using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class ExamResult
{
    public int ResultId { get; set; }

    public int? ExamId { get; set; }

    public int? StudentId { get; set; }

    public decimal? TotalPoint { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual User? Student { get; set; }
}
