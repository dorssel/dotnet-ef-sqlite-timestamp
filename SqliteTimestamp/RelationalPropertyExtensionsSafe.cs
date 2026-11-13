// SPDX-FileCopyrightText: 2026 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dorssel.EntityFrameworkCore;

/// <summary>
/// The signature for <see cref="RelationalPropertyExtensions.GetTableColumnMappings(IProperty)"/> changed between
/// EF Core 10 and EF Core 11. This class provides a safe way to call that method in a way that works for both versions,
/// and is preserved for Native AOT via <see cref="DynamicDependencyAttribute"/>.
///
/// This allows us to have a single library that works for all combinations of .NET + EF Core.
/// </summary>
static class RelationalPropertyExtensionsSafe
{
    static readonly Func<IProperty, IEnumerable<IColumnMapping>> GetTableColumnMappingsDelegate = CreateDelegate();

    /// <inheritdoc cref="RelationalPropertyExtensions.GetTableColumnMappings(IProperty)"/>
    /// <remarks>
    /// EF Core version-independent call. Works for EF Core 8, 9, 10, and 11.
    /// </remarks>
    public static IEnumerable<IColumnMapping> GetTableColumnMappingsSafe(this IProperty property)
    {
        return GetTableColumnMappingsDelegate(property);
    }

    [ExcludeFromCodeCoverage]
    static void ThrowMissingIfNull([NotNull] MethodInfo? method)
    {
        if (method is null)
        {
            throw new MissingMethodException(nameof(RelationalPropertyExtensions), nameof(RelationalPropertyExtensions.GetTableColumnMappings));
        }
    }

    // Tells the Native AOT trimmer to always preserve GetTableColumnMappings on RelationalPropertyExtensions.
    [DynamicDependency(nameof(RelationalPropertyExtensions.GetTableColumnMappings), typeof(RelationalPropertyExtensions))]
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Preserved via DynamicDependency.")]
    static Func<IProperty, IEnumerable<IColumnMapping>> CreateDelegate()
    {
        var method = typeof(RelationalPropertyExtensions).GetMethod(nameof(RelationalPropertyExtensions.GetTableColumnMappings),
            BindingFlags.Public | BindingFlags.Static);
        ThrowMissingIfNull(method);

        return method.CreateDelegate<Func<IProperty, IEnumerable<IColumnMapping>>>();
    }
}
