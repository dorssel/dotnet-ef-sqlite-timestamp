// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

sealed class MultipleRowVersionsDbContext : MemoryDbContext
{
    public const string SecondRowVersionName = "SecondRowVersion";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity(TestTableName, b =>
        {
            b.Property<byte[]>(SecondRowVersionName)
                    .IsRowVersion();
        });
    }
}
