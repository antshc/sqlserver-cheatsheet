using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfSqlPerformance.Test
{
    public class UsersQueryTest
    {
        private readonly ITestOutputHelper _output;
        private const string ConnectionString = "Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";

        public UsersQueryTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GetTopUsersTest()
        {
            Parallel.ForEach(
                Enumerable.Range(0, 10000),
                new ParallelOptions { MaxDegreeOfParallelism = 10 },
                _ =>
                {
                    var options = new DbContextOptionsBuilder<StackOverflowContext>()
                        .UseSqlServer(ConnectionString)
                        .LogTo(_output.WriteLine, LogLevel.Information) // Logs to xUnit
                        .EnableSensitiveDataLogging() // Shows parameter values
                        .Options;

                    using var context = new StackOverflowContext(options);
                    context.Users
                        .Where(u => u.Reputation > 1000)
                        .OrderByDescending(u => u.Reputation)
                        .Take(1000)
                        .ToList();
                });
        }
    }
}