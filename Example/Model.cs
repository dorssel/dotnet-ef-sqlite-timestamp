// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using System.ComponentModel.DataAnnotations;
using Dorssel.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Example;

sealed class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder
            .UseSqlite($"Data Source={DbPath}")
            .UseSqliteTimestamp();   // <<<<<<<<<< Add this line ...
    }
}

sealed class Blog
{
    public int BlogId { get; set; }

    // ... to make this work in SQLite.

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public required string Url { get; set; }

    public List<Post> Posts { get; } = [];
}

sealed class Post
{
    public int PostId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    public required string Title { get; set; }
    public required string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}
