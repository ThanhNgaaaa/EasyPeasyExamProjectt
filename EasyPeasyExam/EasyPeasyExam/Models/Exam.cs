using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPeasyExam.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public string Title { get; set; } = null!;

    public string? ExamDescription { get; set; }

    public int? Duration { get; set; }

    public int? InstructorId { get; set; }

    public string? ExamImage { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    [NotMapped]
    public string ImageSrc { get; set; }

    public bool? IsPublic { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    public virtual User? Instructor { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
