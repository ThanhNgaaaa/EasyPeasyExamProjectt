using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public FlashcardController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/ Flashcard
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<Flashcard>>> GetFlashcard()
        {
            if (_context.Flashcards == null)
            {
                return NotFound();
            }
            return await _context.Flashcards.ToListAsync();
        }

        // GET: api Flashcard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flashcard>> GetFlashcard(int id)
        {
            if (_context.Flashcards == null)
            {
                return NotFound();
            }
            var flashcard = await _context.Flashcards.FindAsync(id);

            if (flashcard == null)
            {
                return NotFound();
            }

            return flashcard;
        }
        
        [HttpGet]
        [Route("GetFlashcardByFcTopic/{id}")]
        public async Task<ActionResult<IEnumerable<Flashcard>>> GetFlashcardsByFCTopic(int id)
        {
            if (_context.Flashcards == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcards
                .Where(c => c.TopicId == id)
                .Select(c => new Flashcard()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    CreatedDate = c.CreatedDate,
                    TopicId=c.TopicId

                })
                .ToListAsync();

            if (flashcard == null)
            {
                return NotFound();
            }

            return flashcard;
        }
        // PUT: api/Flashcard/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlashcard(int id, Flashcard flashcard)
        {
            if (id != flashcard.Id)
            {
                return BadRequest();
            }

            _context.Entry(flashcard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlashcardExists(id))
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

        // POST: api/Flashcard( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flashcard>> PostFlashcard(Flashcard flashcard)
        {
            if (_context.Flashcards == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Flashcard  is null.");
            }
            _context.Flashcards.Add(flashcard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlashcard", new { id = flashcard.Id }, flashcard); 
        }

        
        // DELETE: api/Flashcard/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashcard(int id)
        {
            if (_context.Flashcards == null)
            {
                return NotFound();
            }
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }

            _context.Flashcards.Remove(flashcard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool FlashcardExists(int id)
        {
            return (_context.Flashcards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
