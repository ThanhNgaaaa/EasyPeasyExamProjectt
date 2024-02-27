using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public VideoController(EasyPeasyExamContext context)
        {
            _context = context;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            return await _context.Videos.ToListAsync();
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.FindAsync(id);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }
        [HttpGet]
        [Route("GetVideoByLessonId/{LessonId}")]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideoByLessonId(int lessonId)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.Where(c => c.LessonId == lessonId).ToListAsync();

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }
        // PUT: api/Video/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (id != video.VideoId)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Video( no Id)
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(Video video)
        {
            if (_context.Videos == null)
            {
                return Problem("Entity set 'EasyPeasyExamContext.Courses'  is null.");
            }
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = video.VideoId }, video);
        }

        // DELETE: api/Video/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            if (_context.Videos == null)
            {
                return NotFound();
            }
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        

        private bool VideoExists(int id)
        {
            return (_context.Videos?.Any(e => e.VideoId == id)).GetValueOrDefault();
        }
    }
}
