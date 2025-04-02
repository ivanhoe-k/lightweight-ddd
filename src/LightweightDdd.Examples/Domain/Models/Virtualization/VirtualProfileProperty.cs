// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Models;
using LightweightDdd.Extensions;
using LightweightDdd.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Models.Virtualization
{
    public sealed record VirtualProfileProperty<TProperty> : VirtualProperty<Profile, TProperty, VirtualProfileProperty<TProperty>>
    {
        private VirtualProfileProperty(Expression<Func<Profile, TProperty>> property)
            : base(property)
        {
        }

        public VirtualProfileProperty(string entity, string property, TProperty? value)
           : base(entity, property, value)
        {
        }

        public static VirtualProfileProperty<TProperty> CreateFor(Expression<Func<Profile, TProperty>> property)
        {
            property.ThrowIfNull();

            return new VirtualProfileProperty<TProperty>(property);
        }
    }
}
