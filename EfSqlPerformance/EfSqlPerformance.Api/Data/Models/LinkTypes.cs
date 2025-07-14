using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class LinkTypes
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;
}
