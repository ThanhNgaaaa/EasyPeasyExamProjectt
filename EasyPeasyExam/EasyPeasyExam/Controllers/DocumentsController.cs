using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public DocumentController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/Document
        [HttpGet]
        //[Authorize(Roles = "2")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocument()
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            return await _context.Documents.ToListAsync();
        }

        // GET: apiDocument/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }
        
        // PUT: api/Document/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Document document)
        {
            if (id != document.DocumentId)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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

        // POST: api/Document( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Document>> PostDocument(Document document)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Document  is null.");
            }
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.DocumentId, document } );
        }
        [HttpGet]
        [Route("GetDocumentByLessonId/{LessonId}")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentByLessonId(int lessonId)
        {
            if (_context.Documents == null)
            {
                return NotFound();
            }
            var document = await _context.Documents.Where(d => d.LessonId == lessonId).ToListAsync();

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }
        // DELETE: api/Document/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            if (_context.Documents== null)
            {
                return NotFound();
            }
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.DocumentId == id)).GetValueOrDefault();
        }
    }
}
