using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyPeasyExam.Models;
using Microsoft.Extensions.Hosting;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;
        private readonly IWebHostEnvironment _environment;

        public ExamsController(EasyPeasyExamContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment; 
        }

        // GET: api/Exams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExams()
        {
          if (_context.Exams == null)
          {
              return NotFound("canlas");
          }
            return await _context.Exams.Select(x => new Exam()
            {
                ExamId = x.ExamId,
                Title = x.Title,
                ExamDescription = x.ExamDescription,
                ExamImage = x.ExamImage,
                Duration = x.Duration,
                ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, x.ExamImage),
                InstructorId = x.InstructorId,
            }).ToListAsync();
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(int id)
        {
          if (_context.Exams == null)
          {
              return NotFound();
          }
            var exam = await _context.Exams
                .Where(c => c.ExamId == id)
                .Select(c => new Exam()
                {
                    ExamId = c.ExamId,
                    Title = c.Title,
                    ExamDescription = c.ExamDescription,
                    ExamImage = c.ExamImage,
                    Duration = c.Duration,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, c.ExamImage)
                })
                .FirstOrDefaultAsync();
            if (exam == null)
            {
                return NotFound();
            }
            return exam;
        }
        [HttpGet]
        [Route("GetExamByInstructorId/{InstructorId}")]
        public async Task<ActionResult<IEnumerable<Exam>>> GetExamByInstructorId(int InstructorId)
        {
            if (_context.Exams == null)
            {
                return NotFound();
            }
            var exam = await _context.Exams
                .Where(c => c.InstructorId == InstructorId)
                .Select(c => new Exam()
                {
                    ExamId = c.ExamId,
                    Title = c.Title,
                    ExamDescription = c.ExamDescription,
                    ExamImage = c.ExamImage,
                    Duration = c.Duration,
                    ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, c.ExamImage),
                    InstructorId = c.InstructorId

                })
                .ToListAsync();

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }
        // PUT: api/Exams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExam(int id,[FromForm] Exam exam)
        {
            if (id != exam.ExamId)
            {
                return BadRequest();
            }
            if (exam.ImageFile != null)
            {
                DeleteImage(exam.ExamImage);
                exam.ExamImage = await SaveImage(exam.ImageFile);
            }
            _context.Entry(exam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(id))
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

        // POST: api/Exams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Exam>> PostExam([FromForm]Exam exam)
        {
          if (_context.Exams == null)
          {
              return Problem("Entity set 'EasyPeasyExamContext.Exams'  is null.");
          }
            exam.ExamImage = await SaveImage(exam.ImageFile);
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExam", new { id = exam.ExamId }, exam);
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            if (_context.Exams == null)
            {
                return NotFound();
            }
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            DeleteImage(exam.ExamImage);
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamExists(int id)
        {
            return (_context.Exams?.Any(e => e.ExamId == id)).GetValueOrDefault();
        }
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', 'a');
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
