﻿// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;
using System.Linq.Expressions;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    internal sealed record NullableVirtualDummyProperty<TProperty>
        : NullableVirtualProperty<DummyEntity, TProperty, NullableVirtualDummyProperty<TProperty>>
    {
        private NullableVirtualDummyProperty(string entityName, string propertyName)
            : base(entityName, propertyName) 
        { 
        }

        private NullableVirtualDummyProperty(string entityName, string propertyName, bool hasChanged, TProperty? value)
            : base(entityName, propertyName, hasChanged, value) 
        {
        }
    }
}
