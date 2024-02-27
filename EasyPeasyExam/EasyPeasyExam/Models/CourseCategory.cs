using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class CourseCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
