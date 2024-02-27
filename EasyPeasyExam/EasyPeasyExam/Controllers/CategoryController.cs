using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public CategoryController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<CourseCategory>>> GetCategory()
        {
            if (_context.CourseCategories == null)
            {
                return NotFound();
            }
            return await _context.CourseCategories.ToListAsync();
        }

        // GET: apiCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseCategory>> GetCategory(int id)
        {
            if (_context.CourseCategories == null)
            {
                return NotFound();
            }
            var category = await _context.CourseCategories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
        
        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CourseCategory category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        public async Task<ActionResult<CourseCategory>> PostCategory(CourseCategory category)
        {
            if (_context.CourseCategories == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Category  is null.");
            }
            _context.CourseCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.CourseCategories== null)
            {
                return NotFound();
            }
            var category = await _context.CourseCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.CourseCategories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CategoryExists(int id)
        {
            return (_context.CourseCategories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
