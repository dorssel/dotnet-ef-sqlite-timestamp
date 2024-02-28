// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Example;
using Microsoft.EntityFrameworkCore;

using var db = new BloggingContext();

Console.WriteLine($"Database path: {db.DbPath}.");

Console.WriteLine("Migrating (will create database if it does not exist).");
db.Database.Migrate();

Blog blog;
Post post;

// Create
Console.WriteLine("Inserting a new blog");
blog = new Blog { Url = "https://blogs.msdn.com/adonet" };
db.Add(blog);
db.SaveChanges();
Console.WriteLine($"BlogId = {blog.BlogId}");

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
post = new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" };
blog.Posts.Add(post);
db.SaveChanges();

// Modify
Console.WriteLine("Modifying the post (with row versioning!)");
post.Content = "SQLite will check and update the row version!";
db.Update(post);
db.SaveChanges();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
db.SaveChanges();
