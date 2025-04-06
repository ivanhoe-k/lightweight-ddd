// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    internal sealed record InvalidVirtualDummyProperty
        : VirtualProperty<DummyEntity, int, InvalidVirtualDummyProperty>
    {
        public InvalidVirtualDummyProperty(string entity, string property, int value)
            : base(entity, property, value) 
        {
        }
    }
}
