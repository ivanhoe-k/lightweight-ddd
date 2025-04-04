// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    public sealed record InvalidVirtualDummyProperty
        : VirtualProperty<DummyEntity, int, InvalidVirtualDummyProperty>
    {
        // This will break the framework at runtime
        public InvalidVirtualDummyProperty(Expression<Func<DummyEntity, int>> expression)
            : base(expression) 
        {
        }

        public InvalidVirtualDummyProperty(string entity, string property, int value)
            : base(entity, property, value) 
        {
        }
    }
}
