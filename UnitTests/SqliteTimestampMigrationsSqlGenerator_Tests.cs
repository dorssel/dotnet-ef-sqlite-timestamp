// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampMigrationsSqlGenerator_Tests
{
    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_Create()
    {
        using var db = new MemoryDbContext();

        db.Database.EnsureCreated();
    }
}
