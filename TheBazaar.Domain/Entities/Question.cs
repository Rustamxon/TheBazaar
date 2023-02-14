using TheBazaar.Domain.Commons;
using TheBazaar.Domain.Enums;

namespace TheBazaar.Domain.Entities;

public class Question : Auditable
{
    public long UserId { get; set; }
    public string QuestionText { get; set; }
    public string AnswerText { get; set; }
    public QuestionProgressType Progress { get; set; }
}
