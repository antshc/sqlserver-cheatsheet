using EfSqlPerformance.Test.Fixtures;

namespace EfSqlPerformance.Test
{
    public class UsersQueryTest : QueryTestBase, IClassFixture<DbContextFixture>
    {
        public UsersQueryTest(DbContextFixture contextFixture, ITestOutputHelper output)
            : base(contextFixture, output)
        {
        }

        [Fact]
        public void GetTopUsersTest()
        {
            var topUsers = Context.Users
                .Where(u => u.Reputation > 1000)
                .OrderByDescending(u => u.Reputation)
                .Take(10)
                .ToList();
        }
    }
}