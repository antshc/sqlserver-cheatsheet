using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class Badges
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime Date { get; set; }
}
