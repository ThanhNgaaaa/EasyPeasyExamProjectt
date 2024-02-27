using System;
using System.Collections.Generic;

namespace EasyPeasyExam.Models;

public partial class FlashCardTopic
{
    public int Id { get; set; }

    public string? TopicName { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

    public virtual User? User { get; set; }
}
