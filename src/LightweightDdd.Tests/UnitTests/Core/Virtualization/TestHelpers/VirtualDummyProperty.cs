// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    // Custom leaf virtual property (non-nullable)
    internal sealed record VirtualDummyProperty<TProperty>
        : VirtualProperty<DummyEntity, TProperty, VirtualDummyProperty<TProperty>>
        where TProperty : notnull
    {
        private VirtualDummyProperty(string entityName, string propertyName)
            : base(entityName, propertyName) 
        { 
        }

        private VirtualDummyProperty(string entityName, string propertyName, bool hasChanged, TProperty value)
            : base(entityName, propertyName, hasChanged, value) 
        { 
        }
    }
}
