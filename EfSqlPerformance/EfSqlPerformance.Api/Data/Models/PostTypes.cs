using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class PostTypes
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Posts> Posts { get; set; } = new List<Posts>();
}
