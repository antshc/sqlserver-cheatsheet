using EfSqlPerformance.Api.Application.User;
using EfSqlPerformance.Api.Data;
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
        public async Task GetTopUsersByReputation()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Start();
            await Parallel.ForEachAsync(
                Enumerable.Range(0, 10),
                new ParallelOptions { MaxDegreeOfParallelism = 2 },
                async (_, ct) =>
                {
                    var options = new DbContextOptionsBuilder<StackOverflowContext>()
                        .UseSqlServer(ConnectionString)
                        .LogTo(_output.WriteLine, LogLevel.Information) // Logs to xUnit
                        .EnableSensitiveDataLogging() // Shows parameter values
                        .Options;

                    await using var context = new StackOverflowContext(options);
                    var query = new UsersQuery(context);
                    await query.GetTopUsersByReputation();
                });
            stopwatch.Stop();
            _output.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalSeconds:F2} s ({stopwatch.ElapsedMilliseconds} ms)");
        }
    }
}