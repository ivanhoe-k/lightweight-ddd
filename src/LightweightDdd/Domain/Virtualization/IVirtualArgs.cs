// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#pragma warning disable CA1040 // Avoid empty interfaces
namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    /// Marker interface for all argument types used in virtual entity construction.
    /// </summary>
    /// <remarks>
    /// This interface allows generic constraints on builders or other utilities that operate
    /// on any virtual args type without needing to know the specific builder or entity involved.
    /// For example, <c>BaseVirtualArgsBuilder&lt;TArgs&gt;</c> can constrain <c>TArgs</c> to implement
    /// <see cref="IVirtualArgs"/>, ensuring it’s part of the virtualization pattern.
    /// While <see cref="IVirtualArgs{TEntity, TSelf, TBuilder}"/> defines the full contract,
    /// this non-generic interface enables polymorphic access or reflective discovery.
    /// </remarks>
    public interface IVirtualArgs
    {
    }
}
