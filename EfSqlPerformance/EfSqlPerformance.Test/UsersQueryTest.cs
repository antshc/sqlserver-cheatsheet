using EfSqlPerformance.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace EfSqlPerformance.Test
{
    public class UsersQueryTest
    {
        private const string ConnectionString = "Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";
        private readonly ITestOutputHelper _output;

        public UsersQueryTest(ITestOutputHelper output)
        {
            _output = output;
        }

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