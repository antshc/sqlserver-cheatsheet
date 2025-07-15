using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace EfSqlPerformance.App;

class Program
{
    private const string ConnectionString = "Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";
    private const int DefaultConcurrentUsers = 5;
    private static readonly Random Random = new();
    static readonly string[] PopularTags = new[] { "c#", "sql", "javascript", "asp.net", "entity-framework" };
    static readonly string[] FreeTextQueries = new[] {
        "how to join tables", "async await", "performance tuning", "index usage", "LINQ query", "migration error"
    };

    static void Main(string[] args)
    {
        int concurrentUsers = DefaultConcurrentUsers;
        if (args.Length > 0 && int.TryParse(args[0], out int parsed))
            concurrentUsers = parsed;

        var options = new DbContextOptionsBuilder<StackOverflowContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        var cancellationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            cancellationTokenSource.Cancel();
            e.Cancel = true;
        };

        var tasks = new List<Task>();
        for (int i = 0; i < concurrentUsers; i++)
        {
            int clientId = i + 1;
            tasks.Add(Task.Run(() => SimulateUserInteraction(options, clientId, cancellationTokenSource.Token)));
        }

        Console.WriteLine($"Simulating {concurrentUsers} concurrent users. Press Ctrl+C to stop.");
        Task.WaitAll(tasks.ToArray());
    }

    static async Task SimulateUserInteraction(DbContextOptions<StackOverflowContext> options, int clientId, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            int scenario = Random.Next(5);
            try
            {
                using var context = new StackOverflowContext(options);
                switch (scenario)
                {
                    case 0:
                        ScenarioReadTopUsers(context, clientId);
                        break;
                    case 1:
                        ScenarioInsertUser(context, clientId);
                        break;
                    case 2:
                        ScenarioUpdateUser(context, clientId);
                        break;
                    case 3:
                        ScenarioDeleteUser(context, clientId);
                        break;
                    case 4:
                        ScenarioSearchQuestions(context, clientId);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client {clientId}] Error: {ex.Message}");
            }
            await Task.Delay(Random.Next(500, 1500), token);
        }
    }

    private static void ScenarioReadTopUsers(StackOverflowContext context, int clientId)
    {
        var topUsers = context.Users
            .Where(u => u.Reputation > 1000)
            .OrderByDescending(u => u.Reputation)
            .Take(10)
            .ToList();
        Console.WriteLine($"[Client {clientId}] Read top users: {topUsers.Count}");
    }

    private static void ScenarioInsertUser(StackOverflowContext context, int clientId)
    {
        var newUser = new Users
        {
            DisplayName = $"SimUser_{clientId}_{Guid.NewGuid().ToString().Substring(0, 8)}",
            Reputation = Random.Next(1, 5000),
            CreationDate = DateTime.Now,
            LastAccessDate = DateTime.Now
        };
        context.Users.Add(newUser);
        context.SaveChanges();
        Console.WriteLine($"[Client {clientId}] Inserted user {newUser.DisplayName}");
    }

    private static void ScenarioUpdateUser(StackOverflowContext context, int clientId)
    {
        var userToUpdate = context.Users.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
        if (userToUpdate != null)
        {
            userToUpdate.Reputation += Random.Next(1, 100);
            context.SaveChanges();
            Console.WriteLine($"[Client {clientId}] Updated user {userToUpdate.DisplayName} reputation");
        }
    }

    private static void ScenarioDeleteUser(StackOverflowContext context, int clientId)
    {
        var userToDelete = context.Users
            .Where(u => u.DisplayName.StartsWith("SimUser_"))
            .OrderBy(r => Guid.NewGuid())
            .FirstOrDefault();
        if (userToDelete != null)
        {
            context.Users.Remove(userToDelete);
            context.SaveChanges();
            Console.WriteLine($"[Client {clientId}] Deleted user {userToDelete.DisplayName}");
        }
    }

    private static void ScenarioSearchQuestions(StackOverflowContext context, int clientId)
    {
        int searchType = Random.Next(3);
        IQueryable<Posts> query = context.Posts.Where(p => p.PostTypeId == 1);
        switch (searchType)
        {
            case 0:
                string tag = PopularTags[Random.Next(PopularTags.Length)];
                query = query.Where(p => p.Tags != null && p.Tags.Contains(tag));
                break;
            case 1:
                string text = FreeTextQueries[Random.Next(FreeTextQueries.Length)];
                query = query.Where(p => p.Title.Contains(text) || p.Body.Contains(text));
                break;
            case 2:
                if (Random.Next(2) == 0)
                    query = query.OrderByDescending(p => p.Score);
                else
                    query = query.OrderByDescending(p => p.CreationDate);
                break;
        }
        int take = Random.Next(5, 21);
        var results = query.Take(take).ToList();
        Console.WriteLine($"[Client {clientId}] Searched questions (type {searchType}), results: {results.Count}");
    }
}