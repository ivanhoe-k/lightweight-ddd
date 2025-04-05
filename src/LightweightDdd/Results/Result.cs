// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using LightweightDdd.Extensions;

namespace LightweightDdd.Results
{
    public readonly record struct Result<TError> : IResult<TError>
    {
        private readonly TError _error;
        private readonly bool _initialized;
        private readonly bool _failed;

        private Result(bool initialized)
        {
            _initialized = initialized;
        }

        private Result(TError error) : this(true)
        {
            error.ThrowIfNull();

            _error = error;
            _failed = true;
        }

        public TError Error => ResultGuard.ExtractError(Succeeded, _initialized, _error);

        public bool Failed => ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        public bool Succeeded => !ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        public static Result<TError> Success()
        {
            return new Result<TError>(true);
        }

        public static Result<TError> Fail(TError error)
        {
            return new Result<TError>(error);
        }

        public static Result<TError, TValue> Success<TValue>(TValue value)
        {
            return new Result<TError, TValue>(value);
        }

        public static Result<TError, TValue> Fail<TValue>(TError error)
        {
            return new Result<TError, TValue>(error);
        }
    }

    public readonly record struct Result<TError, TValue> : IResult<TError, TValue>
    {
        private readonly TError _error;
        private readonly TValue _value;
        private readonly bool _initialized;
        private readonly bool _failed;
        private Result(bool initialized)
        {
            _initialized = initialized;
        }

        internal Result(TValue value) : this(true)
        {
            _value = value;
            _failed = false;
        }

        internal Result(TError error) : this(true)
        {
            error.ThrowIfNull();

            _failed = true;
            _error = error;
        }

        public TError Error => ResultGuard.ExtractError(Succeeded, _initialized, _error);

        public TValue Value => ResultGuard.ExtractValue(Succeeded, _initialized, _value);

        public bool Failed => ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);

        public bool Succeeded => !ResultGuard.EnsureInitializedAndReturnFailureState(_failed, _initialized);
    }
}
