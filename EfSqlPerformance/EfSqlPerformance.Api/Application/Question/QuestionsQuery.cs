using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EfSqlPerformance.Api.Application.Question;

public class QuestionsQuery
{
    private readonly StackOverflowContext _context;

    public QuestionsQuery(StackOverflowContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Answer>> GetQuestionAnswers(int? questionId = null, int page = 1, int itemsPerPage = 20)
    {
        return await _context.Posts
            .Where(p => p.ParentId == questionId)
            .OrderByDescending(x => x.LastActivityDate)
            .Skip((page - 1) * itemsPerPage)
            .Take(page * itemsPerPage)
            .Select(p => new Answer()
            {
                AnswerId = p.Id,
                QuestionId  = p.ParentId,
                Title = p.Title,
                Answerer = p.OwnerUser != null ? p.OwnerUser.DisplayName : null
            })
            .ToListAsync();
    }
}