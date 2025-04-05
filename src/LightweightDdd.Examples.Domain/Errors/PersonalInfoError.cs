// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record PersonalInfoError : DomainError<PersonalInfoErrorCode>
    {
        private PersonalInfoError(PersonalInfoErrorCode original) 
            : base(original)
        {
        }

        public static IProfileError MissingFirstName() => new PersonalInfoError(PersonalInfoErrorCode.MissingFirstName);

        public static IProfileError MissingLastName() => new PersonalInfoError(PersonalInfoErrorCode.MissingLastName);

        public static IProfileError InvalidAge() => new PersonalInfoError(PersonalInfoErrorCode.InvalidAge);
    }
}
