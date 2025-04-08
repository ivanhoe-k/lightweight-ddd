// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Virtualization;

namespace LightweightDdd.Tests.UnitTests.Core.Virtualization.TestHelpers
{
    internal sealed class DummyArgsBuilder : VirtualArgsBuilderBase<DummyEntity, DummyEntityArgs>
    {
        public DummyArgsBuilder(DummyEntityArgs args) : base(args) { }

        public DummyArgsBuilder WithAge(int age)
        {
            Args = Args with
            {
                Age = ResolveProperty(Args.Age, age)
            };

            return this;
        }

        public override DummyEntityArgs Build() => Args;
    }
}
