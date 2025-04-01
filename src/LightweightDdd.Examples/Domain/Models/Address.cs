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

        public static Result<IDomainError, Address> Create(string street, string city, string country, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                return Result<IDomainError>.Fail<Address>(AddressError.MissingStreet());
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return Result<IDomainError>.Fail<Address>(AddressError.MissingCity());
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                return Result<IDomainError>.Fail<Address>(AddressError.MissingCountry());
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                return Result<IDomainError>.Fail<Address>(AddressError.MissingPostalCode());
            }

            return Result<IDomainError>.Ok(new Address(
                street: street.Trim(),
                city: city.Trim(),
                country: country.Trim(),
                postalCode: postalCode.Trim()));
        }
    }

}
