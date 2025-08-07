// SPDX-FileCopyrightText: 2025 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class AssemblyTests
{
    [TestMethod]
    public void StrongName()
    {
        var publicKeyToken = typeof(SqliteTimestampDbContextOptionsBuilderExtensions).Assembly.GetName().GetPublicKeyToken();
        Assert.IsNotNull(publicKeyToken);
        Assert.IsGreaterThan(0, publicKeyToken.Length);
    }
}
