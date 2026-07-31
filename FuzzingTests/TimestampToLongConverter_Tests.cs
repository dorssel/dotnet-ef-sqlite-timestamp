// SPDX-FileCopyrightText: 2026 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Dorssel.EntityFrameworkCore.Storage.ValueConversion;

namespace FuzzingTests;

[TestClass]
sealed class TimestampToLongConverter_Tests
{
    [TestMethod]
    public void Reversible()
    {
        var from = TimestampToLongConverter.Singleton.ConvertFromProviderTyped;
        var to = TimestampToLongConverter.Singleton.ConvertToProviderTyped;

        Prop.ForAll<long>(arg => to(from(arg)) == arg).QuickCheckThrowOnFailure();
    }
}
