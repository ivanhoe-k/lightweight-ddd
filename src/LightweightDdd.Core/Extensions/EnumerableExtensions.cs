// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LightweightDdd.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> collection)
           => new ReadOnlyCollection<T>([.. collection]);
    }
}
