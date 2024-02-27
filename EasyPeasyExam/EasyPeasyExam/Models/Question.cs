using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int ExamId { get; set; }

    public string Content { get; set; } = null!;

    public decimal? Point { get; set; }

    public string? QuestionImage { get; set; }

    public string? VideoLink { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();

    public virtual Exam Exam { get; set; } = null!;
}
