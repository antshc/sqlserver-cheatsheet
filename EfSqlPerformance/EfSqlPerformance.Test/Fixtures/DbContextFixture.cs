using EfSqlPerformance.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfSqlPerformance.Test.Fixtures;

public class DbContextFixture : IAsyncDisposable
{
    private const string ConnectionString = "Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";
    public StackOverflowContext Context { get; set; }

    public void Init(Action<string> logTo)
    {
        var options = new DbContextOptionsBuilder<StackOverflowContext>()
            .UseSqlServer(ConnectionString)
            .LogTo(logTo, LogLevel.Information) // Logs to xUnit
            .EnableSensitiveDataLogging() // Shows parameter values
            .Options;

        Context = new StackOverflowContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        if (Context is null) return;

        await Context.DisposeAsync();
    }
}