using EasyPeasyExam.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPeasyExam.Dto
{
    public class CourseDto
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

        public DateTime CreateDate { get; set; }

        public int? CategoryId { get; set; }
        public virtual CourseCategory? Category { get; set; }
        public virtual User? Instructor { get; set; }


    }
}
