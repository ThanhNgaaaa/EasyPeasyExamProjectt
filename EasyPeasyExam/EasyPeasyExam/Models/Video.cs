using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Video
{
    public int VideoId { get; set; }

    public string? Title { get; set; }

    public string? VideoUrl { get; set; }

    public int? LessonId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Lesson? Lesson { get; set; }
}
