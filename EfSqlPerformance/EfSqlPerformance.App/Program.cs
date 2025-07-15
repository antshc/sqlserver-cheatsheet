using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace EfSqlPerformance.App;

class Program
{
    private const string ConnectionString = "Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";
    private const int DefaultConcurrentUsers = 5; // Configurable number of clients
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
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging()
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
            int scenario = Random.Next(5); // Now 5 scenarios
            try
            {
                using var context = new StackOverflowContext(options);
                switch (scenario)
                {
                    case 0:
                        // Scenario 1: Read top users
                        var topUsers = context.Users
                            .Where(u => u.Reputation > 1000)
                            .OrderByDescending(u => u.Reputation)
                            .Take(10)
                            .ToList();
                        Console.WriteLine($"[Client {clientId}] Read top users: {topUsers.Count}");
                        break;
                    case 1:
                        // Scenario 2: Insert a new user
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
                        break;
                    case 2:
                        // Scenario 3: Update a user's reputation
                        var userToUpdate = context.Users.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
                        if (userToUpdate != null)
                        {
                            userToUpdate.Reputation += Random.Next(1, 100);
                            context.SaveChanges();
                            Console.WriteLine($"[Client {clientId}] Updated user {userToUpdate.DisplayName} reputation");
                        }
                        break;
                    case 3:
                        // Scenario 4: Delete a user (only simulated users)
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
                        break;
                    case 4:
                        // Scenario 5: Search Questions
                        int searchType = Random.Next(3);
                        IQueryable<Posts> query = context.Posts.Where(p => p.PostTypeId == 1); // Only questions
                        switch (searchType)
                        {
                            case 0:
                                // Popular tag search
                                string tag = PopularTags[Random.Next(PopularTags.Length)];
                                query = query.Where(p => p.Tags != null && p.Tags.Contains(tag));
                                break;
                            case 1:
                                // Free text search
                                string text = FreeTextQueries[Random.Next(FreeTextQueries.Length)];
                                query = query.Where(p => p.Title.Contains(text) || p.Body.Contains(text));
                                break;
                            case 2:
                                // Advanced filter: sort by votes or newest
                                if (Random.Next(2) == 0)
                                    query = query.OrderByDescending(p => p.Score);
                                else
                                    query = query.OrderByDescending(p => p.CreationDate);
                                break;
                        }
                        int take = Random.Next(5, 21); // Vary result size
                        var results = query.Take(take).ToList();
                        Console.WriteLine($"[Client {clientId}] Searched questions (type {searchType}), results: {results.Count}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client {clientId}] Error: {ex.Message}");
            }
            await Task.Delay(Random.Next(500, 1500), token); // Random delay to simulate user pacing
        }
    }
}

