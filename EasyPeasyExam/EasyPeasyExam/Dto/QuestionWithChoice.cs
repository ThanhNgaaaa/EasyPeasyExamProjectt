using EasyPeasyExam.Models;

namespace EasyPeasyExam.Dto
{
    public class QuestionWithChoice
    {
        public string Content { get; set; }
        public decimal? Point { get; set; }
        public string QuestionImage { get; set; }
        public string VideoLink { get; set; }
        public List<Choice> Choices { get; set; }
    }
}
