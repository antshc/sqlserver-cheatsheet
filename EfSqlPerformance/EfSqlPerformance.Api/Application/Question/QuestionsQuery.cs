using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace EfSqlPerformance.Api.Application.Question;

public class QuestionsQuery
{
    private readonly StackOverflowContext _context;

    public QuestionsQuery(StackOverflowContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Answer>> GetQuestionAnswers(int questionId)
    {
        return await _context.Posts
            .Where(q => q.Id == questionId)
            .SelectMany(q => q.InverseAcceptedAnswer.DefaultIfEmpty(), (q, a) => new { q, a }).Where(res=> res.a.PostTypeId == 1)
            .Select(result => new Answer()
            {
                QuestionId = result.q.Id,
                Title = result.q.Title,
                AnswerId = result.a.Id,
                Answerer = result.a.OwnerUser.DisplayName
            }).ToListAsync();
    }
}