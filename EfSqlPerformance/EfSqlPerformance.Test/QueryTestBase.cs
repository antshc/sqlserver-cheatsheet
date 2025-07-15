using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Test.Fixtures;

namespace EfSqlPerformance.Test;

public class QueryTestBase
{
    public QueryTestBase(DbContextFixture contextFixture, ITestOutputHelper output)
    {
        contextFixture.Init(output.WriteLine);
        Context = contextFixture.Context;
    }

    public StackOverflowContext Context { get; set; }
}