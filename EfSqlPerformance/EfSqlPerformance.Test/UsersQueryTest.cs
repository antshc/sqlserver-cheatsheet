using EfSqlPerformance.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EfSqlPerformance.Test
{
    public class UsersQueryTest
    {
        private const string ConnectionString = "Server=localhost,1433;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";

        [Fact]
        public void GetTopUsersTest()
        {

            var options = new DbContextOptionsBuilder<StackOverflowContext>()
                .UseSqlServer(ConnectionString)
                .Options;

            using var context = new StackOverflowContext(options);

            var topUsers = context.Users
                .Where(u => u.Reputation > 1000)
                .OrderByDescending(u => u.Reputation)
                .Take(10)
                .ToList();
        }
    }
}