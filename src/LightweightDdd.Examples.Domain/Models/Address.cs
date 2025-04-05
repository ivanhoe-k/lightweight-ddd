// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Examples.Domain.Errors;
using LightweightDdd.Results;

namespace LightweightDdd.Examples.Domain.Models
{
    public sealed record Address
    {
        private Address(string street, string city, string country, string postalCode)
        {
            Street = street;
            City = city;
            Country = country;
            PostalCode = postalCode;
        }

        public string Street { get; }

        public string City { get; }

        public string Country { get; }

        public string PostalCode { get; }

        public static Result<IProfileError, Address> Create(string street, string city, string country, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                return Result<IProfileError>.Fail<Address>(AddressError.MissingStreet());
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return Result<IProfileError>.Fail<Address>(AddressError.MissingCity());
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                return Result<IProfileError>.Fail<Address>(AddressError.MissingCountry());
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                return Result<IProfileError>.Fail<Address>(AddressError.MissingPostalCode());
            }

            return Result<IProfileError>.Success(new Address(
                street: street.Trim(),
                city: city.Trim(),
                country: country.Trim(),
                postalCode: postalCode.Trim()));
        }
    }

}
