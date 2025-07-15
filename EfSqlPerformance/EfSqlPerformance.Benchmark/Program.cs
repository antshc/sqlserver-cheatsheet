using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EfSqlPerformance.Api.Data;
using EfSqlPerformance.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EfSqlPerformance.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<EfQueryBenchmark>();

            Console.ReadLine();
        }
    }

    [MemoryDiagnoser]
    public class EfQueryBenchmark
    {
        private StackOverflowContext _context;

        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StackOverflowContext>()
                .UseSqlServer("Server=localhost,14330;Database=StackOverflow2010;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;")
                .Options;

            _context = new StackOverflowContext(options);
        }

        [Benchmark]
        public List<Users> GetTopUsers()
        {
            return _context.Users
                .Where(u => u.Reputation > 1000)
                .OrderByDescending(u => u.Reputation)
                .Take(10)
                .ToList();
        }
    }

}
