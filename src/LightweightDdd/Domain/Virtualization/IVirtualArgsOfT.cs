// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Domain.Virtualization
{
    /// <summary>
    /// Defines a strongly typed contract for all virtual argument types used to partially initialize
    /// a virtualized domain entity, alongside its associated builder.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The domain entity type for which the virtual entity is being constructed.
    /// </typeparam>
    /// <typeparam name="TSelf">
    /// The type of the virtual arguments class itself.
    /// </typeparam>
    /// <typeparam name="TBuilder">
    /// The concrete builder type that can be used to configure and construct this argument instance.
    /// </typeparam>
    /// <remarks>
    /// This interface ensures that:
    /// <list type="bullet">
    /// <item><description><c>TVirtualArgs</c> can be created via a static <c>Create()</c> method</description></item>
    /// <item><description><c>TVirtualArgs</c> exposes a strongly typed builder</description></item>
    /// <item><description>There’s a contractually safe link between args and builder</description></item>
    /// </list>
    ///
    /// This abstraction enables fluent and discoverable construction of argument objects that support
    /// partial domain model initialization without loading the full aggregate from the data source.
    /// </remarks>
    public interface IVirtualArgs<TEntity, TSelf, TBuilder> : IVirtualArgs
        where TEntity : IDomainEntity
        where TSelf : IVirtualArgs<TEntity, TSelf, TBuilder>
        where TBuilder : VirtualArgsBuilderBase<TEntity, TSelf>
    {
        /// <summary>
        /// Returns the strongly typed builder used to populate virtual values fluently.
        /// </summary>
        /// <returns>The corresponding builder instance.</returns>
        static abstract TBuilder GetBuilder();
    }
}
