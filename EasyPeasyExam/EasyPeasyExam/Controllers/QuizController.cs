using EasyPeasyExam.Dto;
using EasyPeasyExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly EasyPeasyExamContext _context;

        public QuizController(EasyPeasyExamContext context)
        {
            _context = context;
        }
        // GET: api/Questions
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Choice>>> GetChoice()
        //{
        //    if (_context.Choices == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.Choices.ToListAsync();
        //}

        //// GET: quiz/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Choice>> GetChoice(int id)
        //{
        //    if (_context.Choices == null)
        //    {
        //        return NotFound();
        //    }
        //    var choice = await _context.Choices.FindAsync(id);

        //    if (choice == null)
        //    {
        //        return NotFound();
        //    }

        //    return choice;
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> GetExam(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return exam;
        }
        //// PUT: api/Quiz/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Question question)
        {
            if (id != question.QuestionId)
            {
                return BadRequest();
            }

            //_context.Entry(question).State = EntityState.Modified;
            var existingQuestion = await _context.Questions
        .Include(q => q.Choices)
        .FirstOrDefaultAsync(q => q.QuestionId == id);

            if (existingQuestion == null)
            {
                return NotFound();
            }
            existingQuestion.Content = question.Content;
            existingQuestion.Point = question.Point;
            existingQuestion.QuestionImage = question.QuestionImage;
            existingQuestion.VideoLink = question.VideoLink;

            foreach (var choice in question.Choices)
            {
                var existingChoice = existingQuestion.Choices.FirstOrDefault(c => c.ChoiceId == choice.ChoiceId);
                if (existingChoice != null)
                {
                    existingChoice.ChoiceContent = choice.ChoiceContent;
                    existingChoice.IsCorrect = choice.IsCorrect;
                    _context.Update(existingChoice);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        //// POST: api/Quiz
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            foreach (var choice in question.Choices)
            {
                choice.QuestionId = question.QuestionId;
                _context.Choices.Add(choice);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.QuestionId }, question);
        }

        [HttpGet]
        [Route("GetQuestionById/{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(int id)
        {
            var question = await _context.Questions.Include(q => q.Choices).FirstOrDefaultAsync(q => q.QuestionId == id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var choices = await _context.Choices.Where(c => c.QuestionId == id).ToListAsync();
            _context.Choices.RemoveRange(choices);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return question;
        }
        private bool QuizExists(int id)
        {
            return (_context.Choices?.Any(e => e.ChoiceId == id)).GetValueOrDefault();
        }
    }
  
}
