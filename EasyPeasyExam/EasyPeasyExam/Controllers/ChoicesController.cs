using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public ChoicesController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/Choices
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<Choice>>> GetChoice()
        {
            if (_context.Choices == null)
            {
                return NotFound();
            }
            return await _context.Choices.ToListAsync();
        }

        // GET: apiChoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Choice>> GetChoice(int id)
        {
            if (_context.Choices == null)
            {
                return NotFound();
            }
            var choice = await _context.Choices.FindAsync(id);

            if (choice == null)
            {
                return NotFound();
            }

            return choice;
        }
        
        // PUT: api/choices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChoice(int id,Choice choice)
        {
            if (id != choice.ChoiceId)
            {
                return BadRequest();
            }

            _context.Entry(choice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoiceExists(id))
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

        // POST: api/Choice( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Choice>> PostChoice(Choice choice)
        {
            if (_context.Choices == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Choice  is null.");
            }
            _context.Choices.Add(choice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChoice", new { id = choice.ChoiceId }, choice);
        }

        // DELETE: api/Choices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            if (_context.Choices== null)
            {
                return NotFound();
            }
            var choice = await _context.Choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }

            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ChoiceExists(int id)
        {
            return (_context.Choices?.Any(e => e.ChoiceId== id)).GetValueOrDefault();
        }
    }
}
