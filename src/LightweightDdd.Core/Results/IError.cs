// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#pragma warning disable CA1040 // Avoid empty interfaces
namespace LightweightDdd.Core.Results
{
    /// <summary>
    /// Marker interface representing an error that can be returned from a domain or infrastructure operation.
    /// </summary>
    /// <remarks>
    /// This interface is typically used in result objects to convey structured failure information
    /// in a consistent and type-safe way across application boundaries.
    /// </remarks>
    public interface IError
    {
    }
}
