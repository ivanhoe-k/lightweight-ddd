// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Virtualization
{
    /// <summary>
    /// Defines the base contract for builders associated with virtual argument objects.
    /// </summary>
    /// <typeparam name="TArgs">
    /// The virtual argument type that the builder constructs.
    /// </typeparam>
    /// <remarks>
    /// Builders following this contract are responsible for producing a fully resolved
    /// instance of <typeparamref name="TArgs"/> by selectively assigning its virtual properties.
    /// </remarks>
    public interface IVirtualArgsBuilder<TArgs>
        where TArgs : IVirtualArgs
    {
        /// <summary>
        /// Finalizes and returns the configured virtual argument instance.
        /// </summary>
        /// <returns>The built <typeparamref name="TArgs"/> instance.</returns>
        TArgs Build();
    }
}
