using System.Linq.Expressions;
using EfSqlPerformance.Api.Application.User.Payload;
using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EfSqlPerformance.Api.Application.User;

public class UsersQuery
{
    private readonly StackOverflowContext _context;

    public UsersQuery(StackOverflowContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserItem>> GetTopUsersByReputation(int top = 200)
    {
        var users = await _context.Users
            .Where(u=>u.Reputation > 100000)
            .OrderByDescending(u => u.Reputation)
            .Take(top)
            .ToListAsync();
        return Map(users);
    }

    private static IEnumerable<UserItem> Map(List<Users> users)
    {
        return users.Select(u => new UserItem(u.Id, u.DisplayName));
    }
}