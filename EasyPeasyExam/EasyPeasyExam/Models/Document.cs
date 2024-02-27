using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? LessonId { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Lesson? Lesson { get; set; }
}
