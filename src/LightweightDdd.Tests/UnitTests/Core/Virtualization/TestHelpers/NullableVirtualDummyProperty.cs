// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    internal sealed record NullableVirtualDummyProperty<TProperty>
        : NullableVirtualProperty<DummyEntity, TProperty, NullableVirtualDummyProperty<TProperty>>
    {
        private NullableVirtualDummyProperty(Expression<Func<DummyEntity, TProperty?>> expression)
            : base(expression) 
        { 
        }

        private NullableVirtualDummyProperty(string entity, string property, TProperty? value)
            : base(entity, property, value) 
        {
        }
    }
}
