using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class VoteTypes
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
