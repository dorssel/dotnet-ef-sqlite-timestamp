// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Metadata;

namespace Dorssel.EntityFrameworkCore;

/// <summary>
/// Extension methods for property metadata.
/// </summary>
public static class SqliteTimestampPropertyExtensions
{
    /// <summary>
    /// Checks whether or not the given property is a <see cref="System.ComponentModel.DataAnnotations.TimestampAttribute"/> property.
    /// </summary>
    /// <param name="property">The property to check.</param>
    /// <returns><see langword="true" /> if the property is a <see cref="System.ComponentModel.DataAnnotations.TimestampAttribute"/> property;
    /// <see langword="false" /> otherwise.</returns>
    public static bool IsSqliteTimestamp(this IReadOnlyProperty property)
    {
        ArgumentNullException.ThrowIfNull(property);

        return (property.ClrType == typeof(byte[])) && property.IsConcurrencyToken && (property.ValueGenerated == ValueGenerated.OnAddOrUpdate);
    }
}
