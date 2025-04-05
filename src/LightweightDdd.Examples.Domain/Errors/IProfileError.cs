// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Domain.Errors;

namespace LightweightDdd.Examples.Domain
{
    public interface IProfileError : IDomainError
    {
        public object Code { get; }
    }
}
