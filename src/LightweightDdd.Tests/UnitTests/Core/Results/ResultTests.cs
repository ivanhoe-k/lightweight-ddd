// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using FluentAssertions;
using LightweightDdd.Results;
using LightweightDdd.Tests.UnitTests.Core.Results.TestHelpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LightweightDdd.Tests.UnitTests.Core.Results
{
    public sealed class ResultTests
    {
        private static readonly DummyError Error = new("oops");
        private const string Value = "hello";

        #region Result<TError>

        [Fact]
        public void Accessing_properties_on_uninitialized_Result_without_value_throws_UninitializedResultException()
        {
            // Arrange
            var result = default(Result<DummyError>);

            // Act
            Action accessSucceeded = () => _ = result.Succeeded;
            Action accessFailed = () => _ = result.Failed;
            Action accessError = () => _ = result.Error;

            // Assert
            accessSucceeded.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);

            accessFailed.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);

            accessError.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);
        }

        [Fact]
        public void Success_ResultWithoutValue_ReturnsCorrectSuccessState()
        {
            // Arrange & Act
            var result = Result<DummyError>.Success();

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Failed.Should().BeFalse();
        }

        [Fact]
        public void Success_ResultWithoutValue_ReturnsSucceeded_And_ThrowsOnErrorAccess()
        {
            // Arrange
            var result = Result<DummyError>.Success();

            // Act
            var succeeded = result.Succeeded;
            var failed = result.Failed;
            Action act = () => _ = result.Error;

            // Assert
            succeeded.Should().BeTrue();
            failed.Should().BeFalse();
            act.Should().Throw<ResultSucceededException>()
               .WithMessage(ResultSucceededException.DefaultMessage);
        }

        [Fact]
        public void Fail_ResultWithoutValue_ReturnsFailed_And_ExposesError()
        {
            // Arrange
            var result = Result<DummyError>.Fail(Error);

            // Act
            var succeeded = result.Succeeded;
            var failed = result.Failed;
            var error = result.Error;

            // Assert
            succeeded.Should().BeFalse();
            failed.Should().BeTrue();
            error.Should().Be(Error);
        }

        #endregion

        #region Result<TError, TValue>

        [Fact]
        public void Accessing_properties_on_uninitialized_Result_with_value_throws_UninitializedResultException()
        {
            // Arrange
            var result = default(Result<DummyError, string>);

            // Act
            Action accessSucceeded = () => _ = result.Succeeded;
            Action accessFailed = () => _ = result.Failed;
            Action accessError = () => _ = result.Error;
            Action accessValue = () => _ = result.Value;

            // Assert
            accessSucceeded.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);

            accessFailed.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);

            accessError.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);

            accessValue.Should()
                .Throw<UninitializedResultException>()
                .WithMessage(UninitializedResultException.DefaultMessage);
        }

        [Fact]
        public void Success_ResultWithValue_ReturnsCorrectValue()
        {
            // Arrange & Act
            var result = Result<DummyError>.Success(Value);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Value.Should().Be(Value);
        }

        [Fact]
        public void Success_ResultWithValue_ReturnsSucceeded_And_ExposesValue()
        {
            // Arrange
            var result = Result<DummyError>.Success(Value);

            // Act
            var succeeded = result.Succeeded;
            var failed = result.Failed;
            var value = result.Value;
            Action act = () => _ = result.Error;

            // Assert
            succeeded.Should().BeTrue();
            failed.Should().BeFalse();
            value.Should().Be(Value);
            act.Should().Throw<ResultSucceededException>()
               .WithMessage(ResultSucceededException.DefaultMessage);
        }

        [Fact]
        public void Fail_ResultWithError_ReturnsFailed_And_ExposesError_And_ThrowsOnValueAccess()
        {
            // Arrange
            var result = Result<DummyError>.Fail<string>(Error);

            // Act
            var succeeded = result.Succeeded;
            var failed = result.Failed;
            var error = result.Error;
            Action act = () => _ = result.Value;

            // Assert
            succeeded.Should().BeFalse();
            failed.Should().BeTrue();
            error.Should().Be(Error);
            act.Should().Throw<ResultFailedException>()
               .WithMessage(ResultFailedException.DefaultMessage);
        }

        #endregion
    }
}
