using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Answer
{
    public int AnswerId { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public string? YourAnswer { get; set; }

    public string CorrectAnswer { get; set; } = null!;

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }
}
