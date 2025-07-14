using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class Votes
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int? UserId { get; set; }

    public int? BountyAmount { get; set; }

    public int VoteTypeId { get; set; }

    public DateTime CreationDate { get; set; }
}
