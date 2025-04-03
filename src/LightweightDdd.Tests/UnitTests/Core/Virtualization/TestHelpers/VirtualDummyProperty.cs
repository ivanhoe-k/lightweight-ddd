// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    // Custom leaf virtual property (non-nullable)
    public sealed record VirtualDummyProperty<TProperty>
        : VirtualProperty<DummyEntity, TProperty, VirtualDummyProperty<TProperty>>
        where TProperty : notnull
    {
        private VirtualDummyProperty(Expression<Func<DummyEntity, TProperty>> expression)
            : base(expression) 
        { 
        }

        private VirtualDummyProperty(string entity, string property, TProperty value)
            : base(entity, property, value) 
        { 
        }
    }
}
