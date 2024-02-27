using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public string? LessonTitle { get; set; }

    public int? CourseId { get; set; }

    public string? LessonType { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
