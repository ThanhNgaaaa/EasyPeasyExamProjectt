using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Authorization;
using EasyPeasyExam.Dto;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;
        private readonly IWebHostEnvironment _environment;

        public CoursesController(EasyPeasyExamContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Courses 
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            return await _context.Courses.Select(x => new Course()
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                CourseDescription = x.CourseDescription,
                CourseImage = x.CourseImage,
                CategoryId = x.CategoryId,
                InstructorId = x.InstructorId,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.CourseImage)
            }).ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Where(c => c.CourseId == id)
                .Select(c => new Course()
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseDescription = c.CourseDescription,
                    CourseImage = c.CourseImage,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, c.CourseImage)
                })
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }
        [HttpGet]
        [Route("GetCourseByInstrId/{InstructorId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourseByInstrId(int InstructorId)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .Where(c => c.InstructorId == InstructorId)
                .Select(c => new Course()
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseDescription = c.CourseDescription,
                    CourseImage = c.CourseImage,
                    CategoryId = c.CategoryId,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, c.CourseImage),
                    InstructorId = c.InstructorId
                })
                .ToListAsync();

            if (courses == null)
            {
                return NotFound();
            }

            return courses;
        }
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id,[FromForm] Course course)
        {
            if (id != course.CourseId)
            {

                return BadRequest("Wrong id" + course.CourseId);
            }
            if (course.ImageFile != null)
            {
                DeleteImage(course.CourseImage);
                course.CourseImage = await SaveImage(course.ImageFile);
            }
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse([FromForm] Course course)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Courses'  is null.");
            }
            course.CourseImage = await SaveImage(course.ImageFile);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            DeleteImage(course.CourseImage);
           
            var lesson = await _context.Lessons.Where(c => c.CourseId == id).ToListAsync();
            _context.Lessons.RemoveRange(lesson);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //SEARCH : /api/course?search=english
        [HttpGet("search")]
        [AllowAnonymous]
        //[Route("SearchCourse")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCourse([FromQuery] string searchKey)
        {
            var courses = await _context.Courses
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchKey))
            {
                courses = courses.Where(c => c.CourseName.ToLower().Contains(searchKey.ToLower()) || c.CourseDescription.ToLower().Contains(searchKey.ToLower())).Select(c => new Course()
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseDescription = c.CourseDescription,
                    CourseImage = c.CourseImage,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, c.CourseImage)
                })
        .ToList();
            }
            else
            {
                return BadRequest("No Course is found");
            }
            return Ok(courses);
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_environment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;

        }
        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_environment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
