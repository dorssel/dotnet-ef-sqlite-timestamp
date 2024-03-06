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
        optionsBuilder
            .UseSqlite("DataSource=:memory:")
            .UseSqliteTimestamp()
            .LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity(TestTableName, b =>
        {
            b.Property<long>(IdName)
                    .ValueGeneratedOnAdd();

            b.Property<byte[]>(RowVersionName)
                    .IsRowVersion();

            b.HasKey(IdName);
        });
    }
}
