// Copyright (c) 2025 Ivan Krepyshev
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LightweightDdd.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightweightDdd.Examples.Domain.Errors
{
    public sealed record AddressError : DomainError<AddressErrorCode>
    {
        private AddressError(AddressErrorCode code) 
            : base(code)
        {
        }

        public static IDomainError MissingStreet() => new AddressError(AddressErrorCode.MissingStreet);

        public static IDomainError MissingCity() => new AddressError(AddressErrorCode.MissingCity);

        public static IDomainError MissingCountry() => new AddressError(AddressErrorCode.MissingCountry);

        public static IDomainError MissingPostalCode() => new AddressError(AddressErrorCode.MissingPostalCode);
    }
}
