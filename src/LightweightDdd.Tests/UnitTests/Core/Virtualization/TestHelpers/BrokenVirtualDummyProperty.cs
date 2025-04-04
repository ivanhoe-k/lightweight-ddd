// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.


// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Virtualization;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    public sealed record BrokenVirtualDummyProperty<T>
        : VirtualProperty<DummyEntity, T, BrokenVirtualDummyProperty<T>>
        where T : notnull
    {
        private BrokenVirtualDummyProperty(string x) : base(x, "", default!) { }
    }
}
