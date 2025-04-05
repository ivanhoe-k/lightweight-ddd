// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Results
{
    /// <summary>
    /// Provides internal guard methods for safely accessing result values and errors.
    /// Throws appropriate exceptions based on the result's state.
    /// </summary>
    internal static class ResultGuard
    {
        /// <summary>
        /// Ensures the result has been initialized before accessing its success or failure state.
        /// Throws <see cref="UninitializedResultException"/> if the result is not properly initialized.
        /// </summary>
        /// <param name="failed">Whether the result has failed internally.</param>
        /// <param name="initialized">Whether the result has been properly initialized.</param>
        /// <returns><c>true</c> if the result has failed; otherwise <c>false</c>.</returns>
        /// <exception cref="UninitializedResultException">
        /// Thrown if the result has not been initialized (e.g., via default struct constructor).
        /// </exception>
        public static bool EnsureInitializedAndReturnFailureState(bool failed, bool initialized)
        {
            if (!initialized)
            {
                throw new UninitializedResultException();
            }

            return failed;
        }

        /// <summary>
        /// Returns the value if the result has succeeded; otherwise, throws
        /// <see cref="ResultFailedException"/> or <see cref="UninitializedResultException"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the result value.</typeparam>
        /// <param name="succeeded">Indicates whether the result has succeeded.</param>
        /// <param name="initialized">Indicates whether the result has been properly initialized.</param>
        /// <param name="value">The result value to return.</param>
        /// <returns>The result value if the result succeeded.</returns>
        /// <exception cref="UninitializedResultException">Thrown if the result is uninitialized.</exception>
        /// <exception cref="ResultFailedException">Thrown if the result has failed.</exception>
        public static TValue ExtractValue<TValue>(bool succeeded, bool initialized, TValue value)
        {
            if (!initialized)
            {
                throw new UninitializedResultException();
            }

            if (!succeeded)
            {
                throw new ResultFailedException();
            }

            return value;
        }

        /// <summary>
        /// Returns the error if the result has failed; otherwise, throws
        /// <see cref="ResultSucceededException"/> or <see cref="UninitializedResultException"/>.
        /// </summary>
        /// <typeparam name="TError">The type of the result error.</typeparam>
        /// <param name="succeeded">Indicates whether the result has succeeded.</param>
        /// <param name="initialized">Indicates whether the result has been properly initialized.</param>
        /// <param name="error">The result error to return.</param>
        /// <returns>The result error if the result has failed.</returns>
        /// <exception cref="UninitializedResultException">Thrown if the result is uninitialized.</exception>
        /// <exception cref="ResultSucceededException">Thrown if the result has succeeded.</exception>
        public static TError ExtractError<TError>(bool succeeded, bool initialized, TError error)
        {
            if (!initialized)
            {
                throw new UninitializedResultException();
            }

            if (succeeded)
            {
                throw new ResultSucceededException();
            }

            return error;
        }
    }
}
