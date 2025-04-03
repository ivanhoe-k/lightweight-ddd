// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Core.Virtualization.Exceptions
{
    /// <summary>
    /// Exception thrown when the virtual property system is unable to resolve a suitable constructor
    /// for the given virtual property type.
    /// </summary>
    /// <remarks>
    /// This typically occurs when the derived virtual property class does not define a matching private or protected constructor
    /// required for reflection-based instantiation. Ensure your leaf virtual property defines both required constructors.
    /// </remarks>
    public sealed class VirtualPropertyConstructorResolutionException : InvalidOperationException
    {
        /// <summary>
        /// Gets the type of the virtual property for which constructor resolution failed.
        /// </summary>
        public Type VirtualPropertyType { get; }

        /// <summary>
        /// Gets the list of constructor parameter types that were attempted.
        /// </summary>
        public Type[] AttemptedParameterTypes { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPropertyConstructorResolutionException"/> class.
        /// </summary>
        /// <param name="virtualPropertyType">The CLR type of the virtual property that failed constructor resolution.</param>
        /// <param name="attemptedParams">The constructor parameter signature that was searched for.</param>
        public VirtualPropertyConstructorResolutionException(Type virtualPropertyType, Type[] attemptedParams)
            : base($"Could not resolve a valid constructor for type '{virtualPropertyType.FullName}'. " +
                   $"Expected constructor with parameters: ({string.Join(", ", attemptedParams.Select(t => t.Name))}).")
        {
            VirtualPropertyType = virtualPropertyType;
            AttemptedParameterTypes = attemptedParams;
        }
    }

}
