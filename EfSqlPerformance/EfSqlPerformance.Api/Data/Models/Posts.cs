﻿using System;
using System.Collections.Generic;

namespace EfSqlPerformance.Api.Data.Models;

public partial class Posts
{
    public int Id { get; set; }

    public int? AcceptedAnswerId { get; set; }

    public int? AnswerCount { get; set; }

    public string Body { get; set; } = null!;

    public DateTime? ClosedDate { get; set; }

    public int? CommentCount { get; set; }

    public DateTime? CommunityOwnedDate { get; set; }

    public DateTime CreationDate { get; set; }

    public int? FavoriteCount { get; set; }

    public DateTime LastActivityDate { get; set; }

    public DateTime? LastEditDate { get; set; }

    public string? LastEditorDisplayName { get; set; }

    public int? LastEditorUserId { get; set; }

    public int? OwnerUserId { get; set; }

    public int? ParentId { get; set; }

    public int PostTypeId { get; set; }

    public int Score { get; set; }

    public string? Tags { get; set; }

    public string? Title { get; set; }

    public int ViewCount { get; set; }

    public virtual Posts? AcceptedAnswer { get; set; }

    public virtual ICollection<Posts> InverseAcceptedAnswer { get; set; } = new List<Posts>();

    public virtual ICollection<Posts> InverseParent { get; set; } = new List<Posts>();

    public virtual Users? OwnerUser { get; set; }

    public virtual Posts? Parent { get; set; }

    public virtual PostTypes PostType { get; set; } = null!;
}
