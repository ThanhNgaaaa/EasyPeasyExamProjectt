using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class Flashcard
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public int? TopicId { get; set; }

    public string? Content { get; set; }

    public virtual FlashCardTopic? Topic { get; set; }
}
