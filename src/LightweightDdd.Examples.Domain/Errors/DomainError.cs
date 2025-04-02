// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Core.Results;
using System;

namespace LightweightDdd.Examples.Domain.Errors
{
    public abstract record DomainError<TErrorCode> : IDomainError
        where TErrorCode : Enum
    {
        protected DomainError(TErrorCode code)
        {
            Code = code;
        }

        public TErrorCode Code { get; }

        object IDomainError.Code => Code;

        public override string ToString()
        {
            return Code.ToString();
        }
    }
}
