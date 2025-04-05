// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record AddressError : DomainError<AddressErrorCode>
    {
        private AddressError(AddressErrorCode code) 
            : base(code)
        {
        }

        public static IProfileError MissingStreet() => new AddressError(AddressErrorCode.MissingStreet);

        public static IProfileError MissingCity() => new AddressError(AddressErrorCode.MissingCity);

        public static IProfileError MissingCountry() => new AddressError(AddressErrorCode.MissingCountry);

        public static IProfileError MissingPostalCode() => new AddressError(AddressErrorCode.MissingPostalCode);
    }
}
