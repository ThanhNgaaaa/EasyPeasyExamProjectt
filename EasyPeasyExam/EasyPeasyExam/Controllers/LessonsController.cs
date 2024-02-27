using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public LessonsController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/Lesson 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLesson()
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            return await _context.Lessons.ToListAsync();
        }

        // GET: api/Lesson/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var lesson = await _context.Lessons.FindAsync(id);


            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }
        [HttpGet]
        [Route("GetLessonData/{LessonId}")]
        public IActionResult GetLessonData(int lessonId)
        {
            var documents = _context.Documents.Where(d => d.LessonId == lessonId).ToList();
            var videos = _context.Videos.Where(v => v.LessonId == lessonId).ToList();

            // Gom các dữ liệu từ Document và Video lại thành một đối tượng
            var lessonData = new
            {
                Documents = documents,
                Videos = videos
            };

            return Ok(lessonData);
        }
        [HttpGet]
        [Route("GetLessonByCourseId/{CourseId}")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonByCourseId(int CourseId)
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var lesson = await _context.Lessons.Where(c => c.CourseId == CourseId).ToListAsync();

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }
        // PUT: api/Lesson/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLesson(int id, Lesson lesson)
        {
            if (id != lesson.LessonId)
            {
                return BadRequest();
            }

            _context.Entry(lesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(id))
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

        // POST: api/Lessons( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
        {
            if (_context.Lessons== null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Courses'  is null.");
            }
            _context.Lessons.Add(lesson);

            //await _context.SaveChangesAsync();
            //foreach (var choice in question.Choices)
            //{
            //    choice.QuestionId = question.QuestionId;
            //    _context.Choices.Add(choice);
            //}
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLesson", new { id = lesson.CourseId }, lesson);
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonExists(int id)
        {
            return (_context.Lessons?.Any(e => e.LessonId == id)).GetValueOrDefault();
        }
    }
}
