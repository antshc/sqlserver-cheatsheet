namespace EfSqlPerformance.Api.Domain;

public class Answer
{
    public int QuestionId { get; set; }
    public int AnswerId { get; set; }
    public string Title { get; set; }
    public string Answerer { get; set; }
}