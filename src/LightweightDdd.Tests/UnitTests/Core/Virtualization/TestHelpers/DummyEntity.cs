// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    public sealed class DummyEntity : DomainEntity<Guid>
    {
        public DummyEntity(Guid id) : base(id) { }

        public string Name => "Test";

        public string? OptionalName => null;

        public int Age => 42;

        public int? Rating => 5;
    }
}
