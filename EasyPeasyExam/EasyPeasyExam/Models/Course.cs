using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPeasyExam.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int? InstructorId { get; set; }

    public string? CourseDescription { get; set; }

    public string? CourseImage { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    [NotMapped]
    public string ImageSrc { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsPublic { get; set; }

    public virtual CourseCategory? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual User? Instructor { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
