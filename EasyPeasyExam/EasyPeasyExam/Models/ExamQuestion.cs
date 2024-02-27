using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class ExamQuestion
{
    public int ExamQuestionId { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }
}
