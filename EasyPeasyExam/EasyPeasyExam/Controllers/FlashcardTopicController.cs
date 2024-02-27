using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardTopicController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public FlashcardTopicController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/ Flashcard
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<FlashCardTopic>>> GetFlashcardTopic()
        {
            if (_context.FlashCardTopics == null)
            {
                return NotFound();
            }
            return await _context.FlashCardTopics.ToListAsync();
        }

        // GET: api Flashcard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlashCardTopic>> GetFlashcardTopic(int id)
        {
            if (_context.FlashCardTopics == null)
            {
                return NotFound();
            }
            var flashcardTopic = await _context.FlashCardTopics.Include(f => f.Flashcards)
               .FirstOrDefaultAsync(e => e.Id == id);

            if (flashcardTopic == null)
            {
                return NotFound();
            }

            return flashcardTopic;
        }
        [HttpGet]
        [Route("GetFlashcardTopicByUserId/{UserId}")]
        public async Task<ActionResult<IEnumerable<FlashCardTopic>>> GetFlashcardsTopicByInstrId(int UserId)
        {
            if (_context.FlashCardTopics == null)
            {
                return NotFound();
            }

            var flashcard = await _context.FlashCardTopics
                .Where(c => c.UserId == UserId)
                .Select(c => new FlashCardTopic()
                {
                    Id = c.Id,
                    TopicName=c.TopicName,
                    UserId = c.UserId
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
        public async Task<IActionResult> PutFlashcardTopic(int id, FlashCardTopic flashcardTopic)
        {
            if (id != flashcardTopic.Id)
            {
                return BadRequest();
            }

            _context.Entry(flashcardTopic).State = EntityState.Modified;

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
        [HttpGet]
        [Route("GetFlashcardByFcTopicByUserId/{UserId}")]
        public async Task<ActionResult<IEnumerable<FlashCardTopic>>> GetFlashcardsByFCTopic(int UserId)
        {
            if (_context.FlashCardTopics == null)
            {
                return NotFound();
            }
            var flashcardTopics = await _context.FlashCardTopics
                   .Include(c => c.Flashcards)
                   .Where(e => e.UserId == UserId)
                   .ToListAsync();


            if (flashcardTopics == null || !flashcardTopics.Any())
            {
                return NotFound();
            }
        

            return flashcardTopics;
        }
        // POST: api/Flashcard( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlashCardTopic>> PostFlashcardTopic(FlashCardTopic flashcardTopic)
        {
            if (_context.FlashCardTopics == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Flashcard  is null.");
            }
            _context.FlashCardTopics.Add(flashcardTopic);
            await _context.SaveChangesAsync();
            foreach (var flashcard in flashcardTopic.Flashcards)
            {
                //flashcard.Id = 0;
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Flashcards ON");

                flashcard.TopicId = flashcardTopic.Id;
                _context.Flashcards.Add(flashcard);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlashcardTopic", new { id = flashcardTopic.Id }, flashcardTopic);

            
        }

        // DELETE: api/Flashcard/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashcardTopic(int id)
        {
            if (_context.FlashCardTopics == null)
            {
                return NotFound();
            }
            var flashcard = await _context.FlashCardTopics.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }

            _context.FlashCardTopics.Remove(flashcard);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool FlashcardExists(int id)
        {
            return (_context.Flashcards?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
