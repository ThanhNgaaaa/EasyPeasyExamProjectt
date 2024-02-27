using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CommentDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
