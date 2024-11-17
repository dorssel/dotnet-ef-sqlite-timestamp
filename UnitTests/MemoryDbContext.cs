// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

class MemoryDbContext : DbContext
{
    public const string TestTableName = "TestTable";
    public const string IdName = "Id";
    public const string RowVersionName = "RowVersion";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder
            .UseSqlite("DataSource=:memory:")
            .UseSqliteTimestamp()
            .LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity(TestTableName, b =>
        {
            _ = b.Property<long>(IdName)
                    .ValueGeneratedOnAdd();

            _ = b.Property<byte[]>(RowVersionName)
                    .IsRowVersion();

            _ = b.HasKey(IdName);
        });
    }
}
