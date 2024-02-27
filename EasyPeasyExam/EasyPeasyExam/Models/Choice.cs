using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Choice
{
    public int ChoiceId { get; set; }

    public string? ChoiceContent { get; set; }

    public string? ImageChoice { get; set; }

    public bool? IsCorrect { get; set; }

    public int? QuestionId { get; set; }

    public virtual Question? Question { get; set; }
}
