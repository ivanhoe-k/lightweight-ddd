// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    internal sealed record DummyEntityArgs : IVirtualArgs<DummyEntity, DummyEntityArgs, DummyArgsBuilder>
    {
        public static DummyArgsBuilder GetBuilder()
        {
            return new DummyArgsBuilder(new DummyEntityArgs());
        }


        public VirtualProperty<DummyEntity, int> Age { get; init; }

        private DummyEntityArgs()
        {
            Age = VirtualProperty<DummyEntity, int>.Unresolved(x => x.Age);
        }
    }
}
